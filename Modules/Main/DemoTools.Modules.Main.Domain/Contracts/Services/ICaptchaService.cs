using SX.Common.Shared.Models;
using System;

namespace DemoTools.Modules.Main.Domain.Contracts.Services
{
    public interface ICaptchaService
    {
        FileData GenerateCaptcha();
        bool VerifyCaptcha(Guid captchaID, string text, bool discardOnSuccess = true);
    }
}
