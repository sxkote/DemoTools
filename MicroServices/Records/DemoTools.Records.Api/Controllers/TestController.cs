using Microsoft.AspNetCore.Mvc;
using SX.Common.Shared.Contracts;
using System;

namespace DemoTools.Records.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        private static readonly string[] Values = new[]
        {
            "Car", "House", "Computer", "Phone", "Coffee", "Icecream", "Welcome"
        };

        private readonly ISettingsProvider _settingsProvider;

        public TestController(ISettingsProvider settingsProvider)
        {
            _settingsProvider = settingsProvider;
        }

        [HttpGet]
        public string Get()
        {
            var rng = new Random();
            return this.GetType().FullName + " - " + Values[rng.Next(Values.Length)]
                + Environment.NewLine + _settingsProvider.GetSettings("CoreApiURL");
        }
    }
}
