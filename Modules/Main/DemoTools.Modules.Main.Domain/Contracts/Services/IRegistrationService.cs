using DemoTools.Modules.Main.Shared.Models.Registration;
using System;

namespace DemoTools.Modules.Main.Domain.Contracts.Services
{
    public interface IRegistrationService
    {
        Guid RegistrationInit(RegistrationModel model);
        void RegistrationConfirm(Guid activityID, string pin);
    }
}
