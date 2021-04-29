using Microsoft.AspNetCore.Mvc;
using System;

namespace DemoTools.Notifications.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        private static readonly string[] Values = new[]
        {
            "Car", "House", "Computer", "Phone", "Coffee", "Icecream", "Welcome"
        };

        [HttpGet]
        public string Get()
        {
            var rng = new Random();
            var value = Values[rng.Next(Values.Length)];

            return this.GetType().FullName + " - " + value;
        }
    }
}
