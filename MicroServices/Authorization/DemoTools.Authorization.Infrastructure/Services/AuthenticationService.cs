using DemoTools.Authorization.Domain.Entities;
using DemoTools.Authorization.Infrastructure.Data;
using DemoTools.Authorization.Shared.Classes;
using Microsoft.EntityFrameworkCore;
using SX.Common.Shared.Contracts;
using SX.Common.Shared.Exceptions;
using SX.Common.Shared.Interfaces;
using SX.Common.Shared.Models;
using SX.Common.Shared.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DemoTools.Authorization.Infrastructure.Services
{
    public class AuthenticationService : IAuthenticationProvider, IDisposable
    {
        public const int EXPIRATION_HOURS = 4;
        public const int CACHE_MINUTES = 10;
        public const int AUTH_CONFIRM_MINUTES = 10;
        public const string ERROR_AUTH = "Пользователь не авторизован!";
        public const string ERROR_PASSWORD = "Логин или пароль не верны!";
        public const string ERROR_BLOCKED = "Пользователь заблокирован!";


        private readonly CoreDbContext _dbContext;
        private readonly ISettingsProvider _settingsProvider;
        private readonly ICacheProvider _cacheProvider;

        public AuthenticationService(CoreDbContext dbContext, ISettingsProvider settingsProvider, ICacheProvider cacheProvider)
        {
            _dbContext = dbContext;
            _settingsProvider = settingsProvider;
            _cacheProvider = cacheProvider;
        }

        protected SecurityPolicy GetSecurityPolicy()
        {
            return _settingsProvider.GetSettings<SecurityPolicy>("SecurityPolicy") ?? SecurityPolicy.DEFAULT;
        }

        protected void SaveTokenToCache(Token token)
        {
            if (_cacheProvider != null && token != null && token.IsValid())
                _cacheProvider.Set($"auth-token:{token.TokenID}", token, new TimeSpan(0, CACHE_MINUTES, 0));
        }

        protected Token LoadTokenFromCache(string code)
        {
            return _cacheProvider.Get<Token>($"auth-token:{code}");
        }

        public IToken Authenticate(string token)
        {
            Token result = this.LoadTokenFromCache(token);
            if (result != null && result.IsValid())
                return result;

            var now = DateTimeOffset.Now;

            var authToken = _dbContext.Set<AuthToken>()
                .SingleOrDefault(t => t.ID.ToString().ToLower() == token.ToLower() && t.Expire > now);

            result = authToken?.GetToken<Token>();
            if (result == null || !result.IsValid())
                return null;

            this.SaveTokenToCache(result);

            return result;
        }

        public IToken Authenticate(string login, string password, string ip = null)
        {
            Token token = null;

            var person = _dbContext.Set<Person>()
               .Include(m => m.User.UserRoles)
               .ThenInclude(ur => ur.Role)
               .ThenInclude(r => r.RolePermissions)
               .ThenInclude(p => p.Permission)
               .SingleOrDefault(m => m.User.Login.ToLower() == login.ToLower());

            if (person == null || person.User == null)
                throw new CustomAuthenticationException(ERROR_PASSWORD);

            //if (person.User.IsBlocked())
            //    throw new CustomAuthenticationException(ERROR_AUTH_BLOCKED);

            var policy = this.GetSecurityPolicy();

            if (!policy.VerifyPassword(password, person.User.Password))
            {
                //var authFailedData = new UserAuthFailedActivityData(person.User.ID, login, password, ip);

                //_monitoringService?.CreateEventLog(null, EventLogType.LoginFailed, authFailedData, login, person.User.ID.ToString());

                //DomainDispatcher.RaiseEvent(new UserLoginFailedEvent(authFailedData));
                throw new CustomAuthenticationException(ERROR_PASSWORD);
            }

            token = BuildToken(person, ip);
            if (token == null)
                throw new CustomAuthenticationException(ERROR_AUTH);

            var authToken = AuthToken.Create(token);
            _dbContext.Set<AuthToken>().Add(authToken);
            _dbContext.SaveChanges();

            //_monitoringService?.CreateEventLog(token, EventLogType.Login, token, token.Login, token.Code);

            return token;
        }

        private void DeleteAuthTokens(Guid userID)
        {
            var now = CommonService.Now;

            var authTokens = _dbContext.Set<AuthToken>()
                .Where(t => t.UserID == userID && t.Expire > now)
                .ToList();

            authTokens.ForEach(t =>
            {
                t.Cancel();
                _dbContext.Set<AuthToken>().Update(t);
                _cacheProvider.Remove($"auth-token:{t.ID}");
            });

            _dbContext.SaveChanges();
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }


        static private Token BuildToken(Person person, string ip = null)
        {
            if (person == null || person.User == null)
                return null;

            var now = CommonService.Now;

            var token = new Token()
            {
                TokenID = CommonService.NewGuid,
                UserID = person.User.ID,
                SubscriptionID = person.SubscriptionID,
                Login = person.User.Login,
                Name = $"{person.Name.First} {person.Name.Last}",
                IP = ip ?? "",
                Date = now,
                Expire = now.AddHours(EXPIRATION_HOURS),
                Roles = GetUserRoles(person.User),
                Permissions = GetUserPermissions(person.User)
            };

            return token;
        }

        static private string[] GetUserRoles(User user)
        {
            var roles = new List<string>();

            if (user.UserRoles != null && user.UserRoles.Count > 0)
                roles.AddRange(user.UserRoles.Select(ur => ur.Role.Name));

            return roles.ToArray();
        }

        static private string[] GetUserPermissions(User user)
        {
            var permissions = new List<string>();

            if (user.UserRoles != null && user.UserRoles.Count > 0)
                permissions.AddRange(user.UserRoles.SelectMany(ur => ur.Role.RolePermissions.Select(rp => rp.Permission.Name)));

            return permissions.ToArray();
        }
    }
}
