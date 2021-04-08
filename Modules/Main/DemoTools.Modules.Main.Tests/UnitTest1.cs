using NUnit.Framework;
using SX.Common.Infrastructure.Models;
using SX.Common.Infrastructure.Services;

namespace DemoTools.Modules.Main.Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestEmail()
        {
            var login = "email@yandex.ru";
            var password = "password";
            var recipient = "sxkote@gmail.com";

            var config = new SMTPServerConfig($"Server=smtp.yandex.ru;Login={login};Password={password};Port=587;SSL=true;From={login}");

            var service = new EmailNotificationService(config);

            service.SendEmail("Test mail", "Test email is already here!", recipient);

        }
    }
}