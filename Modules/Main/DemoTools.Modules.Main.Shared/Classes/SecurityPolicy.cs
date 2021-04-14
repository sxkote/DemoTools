using SX.Common.Shared.Classes;
using SX.Common.Shared.Exceptions;
using SX.Common.Shared.Services;
using System;
using System.Text.RegularExpressions;

namespace DemoTools.Modules.Main.Shared.Classes
{
    /// <summary>
    /// Политика использования паролей в системе
    /// </summary>
    public class SecurityPolicy
    {
        public const string SETTINGS_NAME = "PolicySecurity";

        static public SecurityPolicy DEFAULT
        {
            get
            {
                return new SecurityPolicy()
                {
                    HashMethod = HashType.BCrypt,
                    VerifyMethod = HashType.BCrypt | HashType.MD5 | HashType.None,
                    Validity = 0,
                    MinLength = 6,
                    UseLetters = 0,
                    UseCapitals = 0,
                    UseDigits = 0,
                    UseSpecials = 0,
                    CanReuse = true,
                    BlockAttempts = 10,
                    BlockInterval = 30,
                    BlockPeriod = 60
                };
            }
        }

        [Flags]
        public enum HashType { None = 1, BCrypt = 2, MD5 = 4 };

        /// <summary>
        /// Метод хэширования пароля
        /// </summary>
        public HashType HashMethod { get; set; } = HashType.BCrypt;
        /// <summary>
        /// Метод проверки пароля
        /// </summary>
        public HashType VerifyMethod { get; set; } = HashType.BCrypt | HashType.MD5 | HashType.None;


        /// <summary>
        /// Срок действия пароля до его смены (в днях)
        /// </summary>
        public int Validity { get; set; }


        /// <summary>
        /// Минимальная длина пароля
        /// </summary>
        public int MinLength { get; set; }

        /// <summary>
        /// Минимальное количество букв
        /// </summary>
        public int UseLetters { get; set; }
        /// <summary>
        /// Минимальное количество заглавных букв
        /// </summary>
        public int UseCapitals { get; set; }
        /// <summary>
        /// Минимальное количество цифр
        /// </summary>
        public int UseDigits { get; set; }
        /// <summary>
        /// Минимальное количество спец знаков
        /// </summary>
        public int UseSpecials { get; set; }


        /// <summary>
        /// Возможность повторного использования пароля
        /// </summary>
        public bool CanReuse { get; set; }


        /// <summary>
        /// Кол-во неудачных попыток входа до блокировки учетки
        /// </summary>
        public int BlockAttempts { get; set; }
        /// <summary>
        /// Кол-во минут, в течении которых подсчитывается кол-во неудачных попыток входа
        /// </summary>
        public int BlockInterval { get; set; }
        /// <summary>
        /// Кол-во минут на которые блокируется учетная запись
        /// </summary>
        public int BlockPeriod { get; set; }


        protected SecurityPolicy() { }

        public bool CheckStrength(string password)
        {
            if (String.IsNullOrWhiteSpace(password))
                return false;

            if (password.Length < this.MinLength)
                return false;

            if (this.UseLetters > 0)
                if (this.CountCharacters(password, "a-zA-Z") < this.UseLetters)
                    return false;

            if (this.UseCapitals > 0)
                if (this.CountCharacters(password, "A-Z") < this.UseCapitals)
                    return false;

            if (this.UseDigits > 0)
                if (this.CountCharacters(password, @"\d") < this.UseDigits)
                    return false;

            if (this.UseSpecials > 0)
                if (this.CountCharacters(password, @"\*\^\%\$\#\@\(\)\-\!\?") < this.UseSpecials)
                    return false;

            return true;
        }

        protected int CountCharacters(string input, string mask)
        {
            var filtered = Regex.Replace(input, $"[^{mask}]", "");
            return String.IsNullOrWhiteSpace(filtered) ? 0 : filtered.Length;
        }

        public bool VerifyPassword(string password, string hash)
        {
            // MD5
            if ((this.VerifyMethod & HashType.MD5) != 0)
            {
                if (CommonService.GetMD5(password).Equals(hash, CommonService.StringComparison))
                    return true;
            }

            // bCrypt
            if ((this.VerifyMethod & HashType.BCrypt) != 0)
            {
                if (CommonService.VerifyPassword(password, hash))
                    return true;
            }

            // None
            if ((this.VerifyMethod & HashType.None) != 0)
            {
                if (password.Equals(hash, StringComparison.CurrentCultureIgnoreCase))
                    return true;
            }

            return false;
        }

        public string HashPassword(string password)
        {
            if (this.HashMethod == HashType.BCrypt)
                return CommonService.HashPassword(password);
            else if (this.HashMethod == HashType.MD5)
                return CommonService.GetMD5(password);
            else
                throw new CustomNotImplementedException("Задан не корректный метод хэширования!");
        }

        public string GeneratePassword()
        {
            // кол-во букв
            var useLetters = Math.Max(this.UseLetters, this.UseCapitals);
            // кол-во знаков в пароле
            var length = Math.Max(useLetters + this.UseDigits + this.UseSpecials, this.MinLength);
            // база пороля (со знаками или без)
            var baseLength = this.UseSpecials > 0 ? CoderService.ExtraBaseLength : CoderService.MaxBaseLength;

            var password = "";
            while (!this.CheckStrength(password))
                password = CommonService.GenerateCode(length + 4, baseLength, true);

            return password;
        }

        public DateTimeOffset? NextPasswordDate()
        {
            if (this.Validity <= 0)
                return null;

            return CommonService.Now.AddDays(this.Validity);
        }
    }
}
