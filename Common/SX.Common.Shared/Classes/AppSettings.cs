using SX.Common.Shared.Contracts;
using SX.Common.Shared.Enums;
using SX.Common.Shared.Interfaces;
using SX.Common.Shared.Services;
using System;
using System.Collections.Generic;

namespace SX.Common.Shared.Classes
{
    public class AppSettings
    {
        static public AppSettings Global { get; private set; }

        static AppSettings()
        {
            Global = new AppSettings();
        }



        public IDependencyResolver DependencyResolver { get; set; }

        //protected AppConfig _config;
        //public AppConfig Config
        //{
        //    get
        //    {
        //        if (_config == null)
        //        {
        //            var settingsProvider = this.DependencyResolver.Resolve<ISettingsProvider>();
        //            _config = settingsProvider.GetSettings<AppConfig>("AppConfig");
        //        }
        //        return _config;
        //    }
        //}


        //public AppMode Mode => this.Config.Mode;
        //public bool IsTest() => this.Config.Mode == AppMode.Test;
        //public bool IsProduction() => this.Config.Mode == AppMode.Production;
        //public bool IsDevelopment() => this.Config.Mode == AppMode.Development;
        //public bool CanTrace() => this.Config.Trace;

        public AppSettings() { }



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

    }
}
