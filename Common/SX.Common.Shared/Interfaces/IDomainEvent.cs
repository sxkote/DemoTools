using System;

namespace SX.Common.Shared.Interfaces
{
    /// <summary>
    /// Defines Domain Event to be fired
    /// </summary>
    public interface IDomainEvent
    {
    }

    /// <summary>
    /// Defines the method that can handle Domain Event of type T
    /// </summary>
    /// <typeparam name="T">Domain Event type of IEvent</typeparam>
    public interface IDomainEventHandler<T> : IDisposable
        where T : IDomainEvent
    {
        void Handle(T args);
    }
}
