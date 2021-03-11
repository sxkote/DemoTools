using SX.Common.Shared.Enums;
using System.Collections.Generic;

namespace SX.Common.Shared.Contracts
{
    public interface IDependencyRegistrator
    {
        void RegisterType<TService>();
        void RegisterType<TService, TInterface>(DependencyScope scope = DependencyScope.Scope, List<KeyValuePair<string, object>> parametres = null) where TService : TInterface;
        void RegisterInstance<TInterface>(TInterface service) where TInterface : class;
    }
}
