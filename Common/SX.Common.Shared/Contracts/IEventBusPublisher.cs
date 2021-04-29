using System;

namespace SX.Common.Shared.Contracts
{
    public interface IEventBusPublisher : IDisposable
    {
        void Publish<T>(T data, string queueName = null);
    }
}
