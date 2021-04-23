using DemoTools.Authorization.Shared.Models.Profile;
using System;

namespace DemoTools.Authorization.Domain.Contracts
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
