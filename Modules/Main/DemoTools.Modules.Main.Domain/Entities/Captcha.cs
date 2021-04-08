using SX.Common.Domain.Entities;
using SX.Common.Shared.Models;
using SX.Common.Shared.Services;
using System;

namespace DemoTools.Modules.Main.Domain.Entities
{
    public class Captcha : Entity<Guid>
    {
        public const int CAPTCHA_EXPIRE_MINUTES = 15;
        public const string CAPTCHA_FILENAME = "captcha.jpg";
        public const int CAPTCHA_WIDTH = 200;
        public const int CAPTCHA_HEIGHT = 50;

        public string Text { get; protected set; }
        public DateTimeOffset Expire { get; protected set; }

        private Captcha() { }

        public bool IsExpired()
        {
            return this.Expire < CommonService.Now;
        }

        public bool Verify(string text)
        {
            if (this.IsExpired())
                return false;

            return this.Text.Equals(text, CommonService.StringComparison);
        }

        public void Discard()
        {
            this.Expire = CommonService.Now;
        }

        public FileData GenerateFile()
        {
            var ct = new CaptchaText(this.Text, CAPTCHA_WIDTH, CAPTCHA_HEIGHT);
            return new FileData($"{this.ID}.jpg", ct.GetImageData());
        }

        static public Captcha Create(string text)
        {
            return new Captcha()
            {
                ID = CommonService.NewGuid,
                Text = text,
                Expire = CommonService.Now.AddMinutes(CAPTCHA_EXPIRE_MINUTES)
            };
        }

        static public Captcha Create() => Create(CommonService.GenerateCode(6, 36, false));
    }
}
