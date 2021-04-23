using DemoTools.Core.Domain.Contracts;
using DemoTools.Core.Domain.Entities;
using DemoTools.Core.Infrastructure.Data;
using DemoTools.Core.Shared.Classes;
using DemoTools.Core.Shared.DomainEvents;
using DemoTools.Core.Shared.Models.Profile;
using Microsoft.EntityFrameworkCore;
using SX.Common.Shared.Classes;
using SX.Common.Shared.Contracts;
using SX.Common.Shared.Exceptions;
using SX.Common.Shared.Models;
using SX.Common.Shared.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DemoTools.Core.Infrastructure.Services
{
    public class ProfileService : IProfileService
    {
        public const int DEFAULT_ACTIVITY_EXPIRATION_HOURS = 2;
        public const string DEFAULT_ACTIVITY_TYPE_REGISTRATION = "Registration";
        public const string DEFAULT_ACTIVITY_TYPE_RECOVERY = "PasswordRecovery";
        public const string DEFAULT_USER_ROLES = "User";

        private readonly CoreDbContext _dbContext;
        private readonly ITokenProvider _tokenProvider;

        protected DbSet<Person> Persons => _dbContext.Set<Person>();

        public ProfileService(CoreDbContext dbContext, ITokenProvider tokenProvider)
        {
            _dbContext = dbContext;
            _tokenProvider = tokenProvider;
        }

        protected SecurityPolicy GetSecurityPolicy() => SecurityPolicy.DEFAULT;

        public ProfileModel GetProfile()
        {
            var token = _tokenProvider.GetToken();

            // get Profile Info
            var person = this.Persons
                .Include(t => t.User)
                .FirstOrDefault(t => t.User.ID == token.UserID);

            if (person == null || person.User == null)
                throw new CustomInputException($"User not found!");

            return new ProfileModel()
            {
                PersonID = person.ID,
                SubscriptionID = person.SubscriptionID,
                Login = person.User.Login,
                Email = person.Email,
                NameFirst = person.Name.First,
                NameLast = person.Name.Last,
                Telephone = person.Cellular
            };
        }

        public Guid RegistrationInit(ProfileRegistrationModel model)
        {
            if (model == null)
                throw new CustomArgumentException("Registration data is empty!");

            // input validation
            if (String.IsNullOrWhiteSpace(model.Email))
                throw new CustomInputException("Email is empty!");
            if (String.IsNullOrWhiteSpace(model.Password))
                throw new CustomInputException("Password is empty!");
            if (!model.IsPasswordMatch())
                throw new CustomInputException("Password confirmation is not equal to Passowrd!");

            // security policy
            var securityPolicy = this.GetSecurityPolicy();
            if (!securityPolicy.CheckStrength(model.Password))
                throw new CustomInputException("Password does not match Security Policy!");

            // change valid login
            model.Login = model.GetValidLogin();

            // check if user already exists with login
            var persons = this.Persons
                .Include(t => t.User)
                .Where(p => p.User.Login.ToLower() == model.Login.ToLower())
                .ToList();
            if (persons.Any())
                throw new CustomInputException($"Person with login {model.Login} is already exist!");

            // create activity
            var now = CommonService.Now;
            var pin = CommonService.GeneratePin(10);
            var activity = Activity.Create(now.AddHours(DEFAULT_ACTIVITY_EXPIRATION_HOURS), DEFAULT_ACTIVITY_TYPE_REGISTRATION, model, pin);

            _dbContext.Set<Activity>().Add(activity);
            _dbContext.SaveChanges();

            // raise event to send email
            DomainDispatcher.RaiseEvent(new RegistrationInitDomainEvent()
            {
                Email = model.Email,
                NameFirst = model.NameFirst,
                NameLast = model.NameLast,
                Login = model.Login,
                PIN = pin
            });

            return activity.ID;
        }

        public void RegistrationConfirm(Guid activityID, string pin)
        {
            var activity = _dbContext.Set<Activity>().FirstOrDefault(a => a.ID == activityID);
            if (activity == null || !activity.IsValid() || activity.Type != DEFAULT_ACTIVITY_TYPE_REGISTRATION)
                throw new CustomNotFoundException("Registration data not found. Please, try again!");

            if (!activity.CheckPin(pin))
                throw new CustomNotFoundException("Registration data not found. Please, try again!");

            var model = activity.GetObject<ProfileRegistrationModel>();
            if (model == null)
                throw new CustomNotFoundException("Registration data not found. Please, try again!");

            // security policy to store passwords
            var securityPolicy = this.GetSecurityPolicy();

            // user's default roles
            List<TypeUserRole> roles = new List<TypeUserRole>();
            CommonService.Split(DEFAULT_USER_ROLES).ToList()
                .ForEach(r => roles.Add(_dbContext.Set<TypeUserRole>().FirstOrDefault(role => role.Name.ToLower() == r.ToLower())));

            // create new subscription
            var subscription = Subscription.Create(model.Login);
            _dbContext.Set<Subscription>().Add(subscription);
            // create new person
            var person = Person.Create(CommonService.NewGuid, subscription.ID, new PersonFullName(model.NameFirst, model.NameLast), model.Telephone, model.Email);
            // create new User
            var login = model.GetValidLogin();
            var user = User.Create(person.ID, login, securityPolicy.HashPassword(model.Password), roles);
            person.SetUser(user);

            // discard activity
            activity.Discard();
            _dbContext.Set<Activity>().Update(activity);

            // add person
            this.Persons.Add(person);

            _dbContext.SaveChanges();
        }

        public Guid PasswordRecoveryInit(ProfilePasswordRecoveryModel model)
        {
            // validate model
            if (model == null || String.IsNullOrWhiteSpace(model.Login))
                throw new CustomInputException("Password Recovery model is empty!");
            if (String.IsNullOrWhiteSpace(model.Password))
                throw new CustomInputException("Password is empty!");
            if (!model.IsPasswordMatch())
                throw new CustomInputException("Password Confirmation does not match!");

            // security policy
            var securityPolicy = this.GetSecurityPolicy();
            if (!securityPolicy.CheckStrength(model.Password))
                throw new CustomInputException("Password does not match Security Policy!");

            // get person
            var person = this.Persons
                .Include(t => t.User)
                .FirstOrDefault(p => p.User.Login.ToLower() == model.Login.ToLower());
            if (person == null || person.User == null)
                throw new CustomInputException($"User with login '{model.Login}' not found!");

            model.PersonID = person.ID;

            // create activity
            var now = CommonService.Now;
            var pin = CommonService.GeneratePin(10);
            var activity = Activity.Create(now.AddHours(DEFAULT_ACTIVITY_EXPIRATION_HOURS), DEFAULT_ACTIVITY_TYPE_RECOVERY, model, pin);

            _dbContext.Set<Activity>().Add(activity);
            _dbContext.SaveChanges();

            // raise event to send email
            DomainDispatcher.RaiseEvent(new PasswordRecoveryInitDomainEvent()
            {
                Email = person.Email,
                NameFirst = person.Name.First,
                NameLast = person.Name.Last,
                Login = model.Login,
                PIN = pin
            });

            return activity.ID;
        }

        public void PasswordRecoveryConfirm(Guid activityID, string pin)
        {
            var activity = _dbContext.Set<Activity>().FirstOrDefault(a => a.ID == activityID);
            if (activity == null || !activity.IsValid() || activity.Type != DEFAULT_ACTIVITY_TYPE_RECOVERY)
                throw new CustomNotFoundException("Password Recovery data not found. Please, try again!");

            if (!activity.CheckPin(pin))
                throw new CustomNotFoundException("Password Recovery data not found. Please, try again!");

            var model = activity.GetObject<ProfilePasswordRecoveryModel>();
            if (model == null || model.PersonID == null)
                throw new CustomNotFoundException("Password Recovery data not found. Please, try again!");

            // get person
            var person = this.Persons
                .Include(t => t.User)
                .FirstOrDefault(t => t.ID == model.PersonID.Value);

            // security policy to store passwords
            var securityPolicy = this.GetSecurityPolicy();
            if (securityPolicy == null)
                throw new CustomArgumentException("Security Policy is not provided!");

            // password to use
            var password = model.Password;
            if (String.IsNullOrWhiteSpace(password))
                throw new CustomInputException("Password is empty!");
            if (!securityPolicy.CheckStrength(password))
                throw new CustomInputException("Password does not match securuty policy!");

            // password Hash
            var passwordHash = securityPolicy.HashPassword(password);
            if (!String.IsNullOrWhiteSpace(person.User.Password) && !securityPolicy.CanReuse && passwordHash.Equals(person.User.Password, CommonService.StringComparison))
                throw new CustomInputException("Password was used already!");

            // change password
            person.User.ChangePassword(passwordHash);

            // discard activity
            activity.Discard();
            _dbContext.Set<Activity>().Update(activity);

            // update person
            this.Persons.Update(person);

            _dbContext.SaveChanges();

            // raise event of password change to send email
            DomainDispatcher.RaiseEvent(new PasswordChangedDomainEvent()
            {
                Login = person.User.Login,
                NameFirst = person.Name.First,
                NameLast = person.Name.Last,
                Email = person.Email,
                Password = model.Password
            });
        }

        public void ChangePassword(ProfileChangePasswordModel model)
        {
            var token = _tokenProvider.GetToken();
            if (token == null || !token.IsValid())
                throw new CustomAuthenticationException("User is not authenticated!");

            // password to use
            var password = model?.Password ?? "";

            // validate model
            if (model == null)
                throw new CustomInputException("Change Password model is empty!");
            if (String.IsNullOrWhiteSpace(password))
                throw new CustomInputException("Password is empty!");
            if (!model.IsPasswordMatch())
                throw new CustomInputException("Password Confirmation does not match!");

            // security policy
            var securityPolicy = this.GetSecurityPolicy();
            if (!securityPolicy.CheckStrength(password))
                throw new CustomInputException("Password does not match securuty policy!");

            // get person
            var person = this.Persons
                .Include(t => t.User)
                .FirstOrDefault(t => t.User.ID == token.UserID);
            if (person == null || person.User == null)
                throw new CustomInputException($"User not found!");

            if (!securityPolicy.VerifyPassword(model.PasswordCurrent, person.User.Password))
                throw new CustomInputException("Current Password does not match!");

            // password Hash
            var passwordHash = securityPolicy.HashPassword(password);
            if (!String.IsNullOrWhiteSpace(person.User.Password) && !securityPolicy.CanReuse && passwordHash.Equals(person.User.Password, CommonService.StringComparison))
                throw new CustomInputException("Password was used already!");

            // change password
            person.User.ChangePassword(passwordHash);
            this.Persons.Update(person);
            _dbContext.SaveChanges();

            // raise event of password change to send email
            DomainDispatcher.RaiseEvent(new PasswordChangedDomainEvent()
            {
                Login = person.User.Login,
                NameFirst = person.Name.First,
                NameLast = person.Name.Last,
                Email = person.Email,
                Password = model.Password
            });
        }
    }
}
