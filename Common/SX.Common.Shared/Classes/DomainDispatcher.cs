using SX.Common.Shared.Contracts;
using SX.Common.Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SX.Common.Shared.Classes
{
    public static class DomainDispatcher
    {
        //so that each thread has its own callbacks
        [ThreadStatic]
        private static List<Delegate> _events;

        // Container with Dependency Injections
        public static IDependencyResolver Container => AppSettings.Global.DependencyResolver;

        // Registers a callback for the given domain event
        public static void RegisterEvent<T>(Action<T> callback) where T : IDomainEvent
        {
            if (_events == null)
                _events = new List<Delegate>();

            _events.Add(callback);
        }

        public static void RegisterTask<T>(Action<T> callback) where T : IDomainTask
        {
            if (_events == null)
                _events = new List<Delegate>();

            _events.Add(callback);
        }

        // Clears callbacks passed to Register on the current thread
        public static void ClearCallbacks()
        {
            _events = null;
        }

        // Raises the given domain event
        public static void RaiseEvent<T>(T args) where T : IDomainEvent
        {
            if (Container != null)
            {
                var handlers = Container.ResolveAll<IDomainEventHandler<T>>().ToArray();
                foreach (var handler in handlers)
                {
                    try
                    {
                        handler.Handle(args);
                    }
                    finally
                    {
                        if (handler != null)
                            handler.Dispose();
                    }
                }
            }

            if (_events != null)
            {
                foreach (var action in _events)
                    if (action is Action<T>)
                        ((Action<T>)action)(args);
            }
        }

        // Executes Domain Task 
        public static IDomainTaskResult ExecuteTask<T>(T args)
            where T : IDomainTask
            //where R : IDomainTaskResult
        {
            //var result = default(R);
            IDomainTaskResult result = null;

            if (Container != null)
            {
                var handlers = Container.ResolveAll<IDomainTaskExecutor<T>>().ToArray();
                foreach (var handler in handlers)
                {
                    try
                    {
                        var current = handler.Execute(args);
                        if (result == null)
                            result = current;
                        else if (current != null)
                            result.Merge(current);
                    }
                    finally
                    {
                        if (handler != null)
                            handler.Dispose();
                    }
                }
            }

            if (_events != null)
            {
                foreach (var action in _events)
                    if (action is Action<T>)
                        ((Action<T>)action)(args);
            }

            return result;
        }

    }
}
