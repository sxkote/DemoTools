using DemoTools.Modules.Main.Shared.Models.Profile;
using System;

namespace DemoTools.Modules.Main.Domain.Contracts.Services
{
    public interface IProfileService
    {
        ProfileModel GetProfile();


        Guid RegistrationInit(ProfileRegistrationModel model);
        void RegistrationConfirm(Guid activityID, string pin);


        Guid PasswordRecoveryInit(ProfilePasswordRecoveryModel model);
        void PasswordRecoveryConfirm(Guid activityID, string pin);

        void ChangePassword(ProfileChangePasswordModel model);
    }
}
