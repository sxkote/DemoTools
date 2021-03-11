using System;
using System.Collections.Generic;

namespace SX.Common.Shared.Contracts
{
    public interface IDependencyResolver : IDisposable
    {
        T Resolve<T>();
        T TryResolve<T>();

        IEnumerable<T> ResolveAll<T>();

        IDependencyResolver CreateScope();
    }
}
