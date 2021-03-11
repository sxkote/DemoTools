using SX.Common.Shared.Enums;

namespace SX.Common.Shared.Classes
{
    public class AppConfig
    {
        public AppMode Mode { get; set; }
        public string Name { get; set; }
        public bool Trace { get; set; }
        //public Guid ClientID { get; set; }
        //public Guid PersonID { get; set; }
        //public bool? UseZZZ { get; set; } = true;


        //public AppType GetAppType() => DefineAppType(this.Name);

        //public bool IsBox() => this.GetAppType() == AppType.Box;

        //static public AppType DefineAppType(string value)
        //{
        //    if (value.Equals("cloud", StringComparison.OrdinalIgnoreCase) || value.Equals("lv", StringComparison.OrdinalIgnoreCase) || value.ToLower().StartsWith("lv-"))
        //        return AppType.LV;

        //    return AppType.Box;
        //}
    }
}
