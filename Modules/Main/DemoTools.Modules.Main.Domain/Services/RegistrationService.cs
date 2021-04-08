using DemoTools.Modules.Main.Domain.Contracts;
using DemoTools.Modules.Main.Domain.Contracts.Repositories;
using DemoTools.Modules.Main.Domain.Contracts.Services;
using DemoTools.Modules.Main.Domain.Entities;
using DemoTools.Modules.Main.Domain.Entities.Persons;
using DemoTools.Modules.Main.Shared.Classes;
using DemoTools.Modules.Main.Shared.DomainEvents;
using DemoTools.Modules.Main.Shared.Models.Registration;
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
    public class RegistrationService : IRegistrationService
    {
        public const string DEFAULT_ACTIVITY_TYPE_REGISTRATION = "Registration";
        public const string DEFAULT_USER_ROLES = "User";

        protected IMainUnitOfWork _unitOfWork;

        public IMainUnitOfWork UnitOfWork => _unitOfWork;
        public IPersonRepository PersonRepository => this.UnitOfWork.PersonRepository;

        public RegistrationService(IMainUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        protected SecurityPolicy GetSecurityPolicy() => SecurityPolicy.DEFAULT;

        public Guid RegistrationInit(RegistrationModel model)
        {
            if (model == null)
                throw new CustomArgumentException("Registration data is empty!");

            // input validation
            if (String.IsNullOrWhiteSpace(model.Email))
                throw new CustomInputException("Email is empty!");
            if (String.IsNullOrWhiteSpace(model.Password))
                throw new CustomInputException("Password is empty!");
            if (model.Password != model.PasswordConfirm)
                throw new CustomInputException("Password confirmation is not equal to Passowrd!");

            // change valid login
            model.Login = model.GetValidLogin();

            // check if user already exists with login
            var person = this.PersonRepository.GetByLogin(model.Login);
            if (person != null)
                throw new CustomInputException($"Person with login {model.Login} is already exist!");

            // create activity
            var now = CommonService.Now;
            var pin = CommonService.GeneratePin(10);
            var activity = Activity.Create<RegistrationModel>(now.AddHours(2), DEFAULT_ACTIVITY_TYPE_REGISTRATION, model, pin);

            this.UnitOfWork.AddEntity<Activity>(activity);
            this.UnitOfWork.SaveChanges();

            // raise event to send email
            DomainDispatcher.RaiseEvent(new PersonRegistrationInited()
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
            if (activity == null || !activity.IsValid())
                throw new CustomNotFoundException("Registration data not found. Please, try registration process again!");

            if (!activity.CheckPin(pin))
                throw new CustomNotFoundException("Registration data not found. Please, try registration process again!");

            var model = activity.GetObject<RegistrationModel>();
            if (model == null)
                throw new CustomNotFoundException("Registration data not found. Please, try registration process again!");

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

            this.PersonRepository.Add(person);
            this.UnitOfWork.SaveChanges();
        }
    }
}
