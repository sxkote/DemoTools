using SX.Common.Shared.Contracts;
using SX.Common.Shared.Enums;
using SX.Common.Shared.Interfaces;
using SX.Common.Shared.Services;
using System;
using System.Collections.Generic;

namespace SX.Common.Shared.Classes
{
    public class AppSettings : ISettingsProvider
    {
        static public AppSettings Global { get; private set; }

        public IDependencyResolver DependencyResolver { get; set; }

        protected AppConfig _application;
        public AppConfig Application
        {
            get
            {
                if (_application == null)
                    _application = this.GetSettings<AppConfig>("AppConfig");
                return _application;
            }
        }

        //public bool IsTest() => this.Application.Mode  == AppMode.Test;
        public bool IsProduction() => this.Application.Mode == AppMode.Production;
        public bool IsDevelopment() => this.Application.Mode == AppMode.Development;
        public bool CanTrace() => this.Application.Trace;

        //public Guid DefaultClientID => this.Application.ClientID;
        //public Guid DefaultPersonID => this.Application.PersonID;
        //public bool UseZZZ => this.Application.UseZZZ == null || this.Application.UseZZZ.Value;

        static AppSettings()
        {
            Global = new AppSettings();
        }

        protected AppSettings() { }



        public string GetSettings(string name)
        {
            return System.Configuration.ConfigurationManager.AppSettings[name];
        }

        public T GetSettings<T>(string name)
        {
            try
            {
                return CommonService.Deserialize<T>(this.GetSettings(name));
            }
            catch { return default(T); }
        }



        static protected AppMode DefineAppMode(string value)
        {
            if (value.Equals("production", StringComparison.OrdinalIgnoreCase) || value.Equals("prod", StringComparison.OrdinalIgnoreCase))
                return AppMode.Production;
            if (value.Equals("test", StringComparison.OrdinalIgnoreCase))
                return AppMode.Test;

            return AppMode.Development;
        }

        static public string GetGlobalSettings(string name)
        {
            return AppSettings.Global.GetSettings(name);
        }

        static public T GetGlobalSettings<T>(string name)
        {
            return AppSettings.Global.GetSettings<T>(name);
        }

        static public T Resolve<T>()
        {
            var resolver = AppSettings.Global.DependencyResolver;

            if (resolver == null)
                return default(T);

            return resolver.Resolve<T>();
        }

        static public IEnumerable<T> ResolveAll<T>()
        {
            var resolver = AppSettings.Global.DependencyResolver;

            if (resolver == null)
                return new List<T>();

            return resolver.ResolveAll<T>();
        }


        static public ILogger GetLogger() => AppSettings.Resolve<ILogger>();
        //static public IMonitoring GetMonitoring() => AppSettings.Resolve<IMonitoring>();
    }
}
