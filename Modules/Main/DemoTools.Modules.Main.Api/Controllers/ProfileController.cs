using DemoTools.Modules.Main.Api.DTO;
using DemoTools.Modules.Main.Api.DTO.Registration;
using DemoTools.Modules.Main.Domain.Contracts.Services;
using DemoTools.Modules.Main.Domain.Entities.Todo;
using DemoTools.Modules.Main.Shared.Models.Registration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace DemoTools.Modules.Main.Api.Controllers
{
    [Authorize(AuthenticationSchemes = "Token", Roles = "User")]
    [Route("api/profile")]
    public class ProfileController : ControllerBase
    {
        private readonly IRegistrationService _registrationService;

        public ProfileController(IRegistrationService registrationService)
        {
            _registrationService = registrationService;
        }


        [AllowAnonymous]
        [HttpPost]
        [Route("registration-init")]
        public RegistrationInitResponseDTO RegistrationInit([FromBody] RegistrationModel model)
        {
            var activityID = _registrationService.RegistrationInit(model);
            return new RegistrationInitResponseDTO() { ActivityID = activityID };
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("registration-confirm/{activityID}/{pin}")]
        public void RegistrationConfirm(Guid activityID, string pin)
        {
            _registrationService.RegistrationConfirm(activityID, pin);
        }
    }
}
