using DemoTools.Modules.Main.Domain.Contracts;
using DemoTools.Modules.Main.Domain.Contracts.Repositories;
using DemoTools.Modules.Main.Domain.Contracts.Services;
using DemoTools.Modules.Main.Domain.Entities;
using DemoTools.Modules.Main.Domain.Entities.Persons;
using DemoTools.Modules.Main.Shared.Classes;
using DemoTools.Modules.Main.Shared.DomainEvents;
using DemoTools.Modules.Main.Shared.Models.Profile;
using SX.Common.Domain.Services;
using SX.Common.Shared.Classes;
using SX.Common.Shared.Contracts;
using SX.Common.Shared.Exceptions;
using SX.Common.Shared.Models;
using SX.Common.Shared.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DemoTools.Modules.Main.Domain.Services
{
    public class ProfileService : DomainService<IMainUnitOfWork>, IProfileService
    {
        public const int DEFAULT_ACTIVITY_EXPIRATION_HOURS = 2;
        public const string DEFAULT_ACTIVITY_TYPE_REGISTRATION = "Registration";
        public const string DEFAULT_ACTIVITY_TYPE_RECOVERY = "PasswordRecovery";
        public const string DEFAULT_USER_ROLES = "User";

        public IPersonRepository PersonRepository => this.UnitOfWork.PersonRepository;

        public ProfileService(IMainUnitOfWork unitOfWork, ITokenProvider tokenProvider)
            :base(unitOfWork, tokenProvider)
        {
            _unitOfWork = unitOfWork;
            _tokenProvider = tokenProvider;
        }

        protected SecurityPolicy GetSecurityPolicy() => SecurityPolicy.DEFAULT;

        public ProfileModel GetProfile()
        {
            var token = this.CheckUser();

            // get person
            var person = this.PersonRepository.GetTracking(token.UserID);
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
            var persons = this.PersonRepository.GetByLogin(model.Login);
            if (persons.Any())
                throw new CustomInputException($"Person with login {model.Login} is already exist!");

            // create activity
            var now = CommonService.Now;
            var pin = CommonService.GeneratePin(10);
            var activity = Activity.Create(now.AddHours(DEFAULT_ACTIVITY_EXPIRATION_HOURS), DEFAULT_ACTIVITY_TYPE_REGISTRATION, model, pin);

            this.UnitOfWork.AddEntity<Activity>(activity);
            this.UnitOfWork.SaveChanges();

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
            var activity = this.UnitOfWork.GetEntity<Activity>(activityID);
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
                .ForEach(r => roles.Add(this.UnitOfWork.TypeUserRoleRepository.Get(r)));

            // create new subscription
            var subscription = Subscription.Create(model.Login);
            this.UnitOfWork.AddEntity<Subscription>(subscription);
            // create new person
            var person = Person.Create(subscription.ID, new PersonFullName(model.NameFirst, model.NameLast), model.Telephone, model.Email, model.Gender, model.BirthDate);
            // create new User
            var login = model.GetValidLogin();
            var user = User.Create(person.ID, login, securityPolicy.HashPassword(model.Password), roles);
            person.SetUser(user);

            // discard activity
            activity.Discard();
            this.UnitOfWork.UpdateEntity(activity);

            // add person
            this.PersonRepository.Add(person);

            this.SaveChanges();
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
            var person = this.PersonRepository.GetByLogin(model.Login).FirstOrDefault();
            if (person == null || person.User == null)
                throw new CustomInputException($"User with login '{model.Login}' not found!");

            model.PersonID = person.ID;

            // create activity
            var now = CommonService.Now;
            var pin = CommonService.GeneratePin(10);
            var activity = Activity.Create(now.AddHours(DEFAULT_ACTIVITY_EXPIRATION_HOURS), DEFAULT_ACTIVITY_TYPE_RECOVERY, model, pin);

            this.UnitOfWork.AddEntity<Activity>(activity);
            this.UnitOfWork.SaveChanges();

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
            var activity = this.UnitOfWork.GetEntity<Activity>(activityID);
            if (activity == null || !activity.IsValid() || activity.Type != DEFAULT_ACTIVITY_TYPE_RECOVERY)
                throw new CustomNotFoundException("Password Recovery data not found. Please, try again!");

            if (!activity.CheckPin(pin))
                throw new CustomNotFoundException("Password Recovery data not found. Please, try again!");

            var model = activity.GetObject<ProfilePasswordRecoveryModel>();
            if (model == null || model.PersonID == null)
                throw new CustomNotFoundException("Password Recovery data not found. Please, try again!");

            // get person
            var person = this.PersonRepository.GetTracking(model.PersonID.Value);

            // security policy to store passwords
            var securityPolicy = this.GetSecurityPolicy();

            // change password
            person.User.ChangePassword(securityPolicy, model.Password);

            // discard activity
            activity.Discard();
            this.UnitOfWork.UpdateEntity(activity);

            // update person
            this.PersonRepository.Update(person);

            this.SaveChanges();

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
            var token = this.CheckUser();

            // validate model
            if (model == null)
                throw new CustomInputException("Change Password model is empty!");
            if (String.IsNullOrWhiteSpace(model.Password))
                throw new CustomInputException("Password is empty!");
            if (!model.IsPasswordMatch())
                throw new CustomInputException("Password Confirmation does not match!");

            // security policy
            var securityPolicy = this.GetSecurityPolicy();
            if (!securityPolicy.CheckStrength(model.Password))
                throw new CustomInputException("Password does not match Security Policy!");

            // get person
            var person = this.PersonRepository.GetTracking(token.UserID);
            if (person == null || person.User == null)
                throw new CustomInputException($"User not found!");

            if (!securityPolicy.VerifyPassword(model.PasswordCurrent, person.User.Password))
                throw new CustomInputException("Current Password does not match!");

            // change password
            person.User.ChangePassword(securityPolicy, model.Password);

            this.PersonRepository.Update(person);
            this.SaveChanges();

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
