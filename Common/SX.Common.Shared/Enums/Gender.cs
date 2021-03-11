using System.ComponentModel.DataAnnotations;

namespace SX.Common.Shared.Enums
{
    public enum Gender : byte
    {
        [Display(Name = "Неизвестно")]
        Unknown = 0,

        [Display(Name = "Мужской")]
        Male = 1,

        [Display(Name = "Женский")]
        Female = 2
    }
}
