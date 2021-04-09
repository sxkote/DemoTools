using DemoTools.Modules.Main.Domain.Contracts.Services;
using DemoTools.Modules.Main.Shared.Models.Profile;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace DemoTools.Modules.Main.Api.Controllers
{
    [Authorize(AuthenticationSchemes = "Token", Roles = "User")]
    [Route("api/profile")]
    public class ProfileController : ControllerBase
    {
        private readonly IProfileService _profileService;

        public ProfileController(IProfileService registrationService)
        {
            _profileService = registrationService;
        }

        [HttpGet]
        [Route("")]
        public ProfileModel GetProfile()
        {
            return _profileService.GetProfile();
        }


        [AllowAnonymous]
        [HttpPost]
        [Route("registration-init")]
        public Guid RegistrationInit([FromBody] ProfileRegistrationModel model)
        {
            return _profileService.RegistrationInit(model);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("registration-confirm/{activityID}/{pin}")]
        public void RegistrationConfirm(Guid activityID, string pin)
        {
            _profileService.RegistrationConfirm(activityID, pin);
        }


        [AllowAnonymous]
        [HttpPost]
        [Route("password-recovery-init")]
        public Guid PasswordRecoveryInit([FromBody] ProfilePasswordRecoveryModel model)
        {
            return _profileService.PasswordRecoveryInit(model);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("password-recovery-confirm/{activityID}/{pin}")]
        public void PasswordRecoveryConfirm(Guid activityID, string pin)
        {
            _profileService.PasswordRecoveryConfirm(activityID, pin);
        }


        [HttpPost]
        [Route("password-change")]
        public void ChangePassword([FromBody] ProfileChangePasswordModel model)
        {
            _profileService.ChangePassword(model);
        }
    }
}
