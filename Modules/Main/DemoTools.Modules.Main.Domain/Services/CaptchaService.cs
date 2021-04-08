using DemoTools.Modules.Main.Domain.Contracts;
using DemoTools.Modules.Main.Domain.Contracts.Services;
using DemoTools.Modules.Main.Domain.Entities;
using SX.Common.Shared.Models;
using System;

namespace DemoTools.Modules.Main.Domain.Services
{
    public class CaptchaService : ICaptchaService
    {
        protected IMainUnitOfWork _unitOfWork;
        public CaptchaService(IMainUnitOfWork uow)
        {
            _unitOfWork = uow;
        }

        public FileData GenerateCaptcha()
        {
            var captcha = Captcha.Create();
            _unitOfWork.AddEntity(captcha);
            _unitOfWork.SaveChanges();

            return captcha.GenerateFile();
        }

        public bool VerifyCaptcha(Guid captchaID, string text, bool discardOnSuccess = true)
        {
            var captcha = _unitOfWork.GetEntity<Captcha>(captchaID);
            if (captcha == null)
                return false;

            var result = captcha.Verify(text);

            if (result && discardOnSuccess)
            {
                captcha.Discard();
                _unitOfWork.SaveChanges();
            }

            return result;
        }
    }
}
