using Microsoft.Extensions.DependencyInjection;
using SX.Common.Shared.Contracts;
using System;
using System.Collections.Generic;

namespace SX.Common.Infrastructure.Services
{
    public class ServiceResolver : IDependencyResolver
    {
        protected IServiceScope _scope = null;
        protected IServiceProvider _serviceProvider;

        public ServiceResolver(IServiceProvider services, IServiceScope scope = null)
        {
            _serviceProvider = services;
            _scope = scope;
        }

        public IDependencyResolver CreateScope()
        {
            var scope = _serviceProvider.CreateScope();
            return new ServiceResolver(scope?.ServiceProvider, scope);
        }

        public void Dispose()
        {
            if (_scope != null)
            {
                _scope.Dispose();
                _scope = null;
            }
        }

        public T Resolve<T>()
        {
            return _serviceProvider.GetRequiredService<T>();
        }

        public IEnumerable<T> ResolveAll<T>()
        {
            return _serviceProvider.GetServices<T>();
        }

        public T TryResolve<T>()
        {
            return _serviceProvider.GetService<T>();
        }
    }
}
