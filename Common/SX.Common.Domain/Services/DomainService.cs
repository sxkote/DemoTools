using SX.Common.Domain.Contracts;
using SX.Common.Domain.Interfaces;
using SX.Common.Shared.Contracts;
using SX.Common.Shared.Exceptions;
using SX.Common.Shared.Interfaces;
using SX.Common.Shared.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SX.Common.Domain.Services
{
    public abstract class DomainService
    {
        public const string EXCEPTION_NOT_USER = "Текущий пользователь не авторизован в системе!";
        public const string EXCEPTION_NOT_ADMIN = "Данное действие доступно исключительно администраторам!";
        public const string EXCEPTION_NO_PERMISSIONS_OBJECT = "Недостаточно прав для доступа к выбранным данным!";
        public const string EXCEPTION_NO_PERMISSIONS_ACTION = "Недостаточно прав для выполнения данной операции!";

        protected ITokenProvider _tokenProvider;

        protected IToken _token;

        public IToken GetToken()
        {
            if (_token == null)
                _token = _tokenProvider?.GetToken();
            return _token;
        }

        public DomainService(ITokenProvider tokenProvider)
        {
            _tokenProvider = tokenProvider;
        }

        protected bool IsTokenValid(IToken token)
        {
            return token != null && token.IsValid();
        }

        protected bool IsAuthenticated()
        {
            return this.IsTokenValid(this.GetToken());
        }

        protected IToken CheckUser()
        {
            var token = this.GetToken();
            if (!this.IsTokenValid(token))
                throw new CustomAuthenticationException(EXCEPTION_NOT_USER);

            return token;
        }

        protected bool IsAdministrator()
        {
            var token = this.GetToken();
            return this.IsTokenValid(token) && token.IsAdministrator();
        }

        protected void CheckAdministrator(string message = "")
        {
            if (!this.IsAdministrator())
                throw new CustomAccessException(String.IsNullOrWhiteSpace(message) ? EXCEPTION_NOT_ADMIN : message);
        }



        protected bool HasPermissions(string permissions)
        {
            var token = this.CheckUser();
            if (token == null)
                return false;

            if (token.IsAdministrator())
                return true;

            //if (clientID != null && token.ClientID != clientID)
            //    return false;

            if (String.IsNullOrWhiteSpace(permissions))
                return true;

            return permissions.Split(new char[] { ',', ';' })
                .All(perm => token.Permissions.Any(p => p.Equals(perm, CommonService.StringComparison)));
        }

        protected void CheckPermissions(string permissions, string message = "")
        {
            if (!this.HasPermissions(permissions))
                throw new CustomAccessException(String.IsNullOrWhiteSpace(message) ? EXCEPTION_NO_PERMISSIONS_ACTION : message);
        }

        //protected void CheckPermissions(string permissions, Guid clientID, string message = "")
        //{
        //    var token = this.CheckUser();

        //    if (token.IsAdministrator())
        //        return;

        //    if (token.ClientID != clientID || !this.HasPermissions(permissions))
        //        throw new CustomAccessException(String.IsNullOrWhiteSpace(message) ? EXCEPTION_NO_PERMISSIONS_ACTION : message);
        //}
    }

    public abstract class DomainService<TUnitOfWork> : DomainService, IDomainService<TUnitOfWork>
       where TUnitOfWork : IDomainUnitOfWork
    {
        protected TUnitOfWork _unitOfWork;

        public TUnitOfWork UnitOfWork { get { return _unitOfWork; } }

        public DomainService(TUnitOfWork unitOfWork, ITokenProvider tokenProvider)
            : base(tokenProvider)
        {
            _unitOfWork = unitOfWork;
        }

        protected void SaveChanges()
        {
            this.UnitOfWork.SaveChanges();
        }

        public virtual void Dispose()
        {
            this.UnitOfWork.Dispose();
        }

        protected bool HasAccess<T>(IEnumerable<T> items, string permissions)
            where T : IAccessibleEntity
        {
            var token = this.CheckUser();
            if (token == null)
                return false;

            if (this.IsAdministrator() || items == null)
                return true;

            if (!this.HasPermissions(permissions))
                return false;

            return items.All(i => i.HasAccess(token));
        }

        protected void CheckAccess<T>(IEnumerable<T> items, string permissions, string error = "")
            where T : IAccessibleEntity
        {
            if (!this.HasAccess<T>(items, permissions))
                throw new CustomAccessException(String.IsNullOrWhiteSpace(error) ? EXCEPTION_NO_PERMISSIONS_OBJECT : error);
        }
    }
}
