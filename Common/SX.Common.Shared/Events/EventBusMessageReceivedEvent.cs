using SX.Common.Shared.Interfaces;

namespace SX.Common.Shared.Events
{
    public class EventBusMessageReceivedEvent : IDomainEvent
    {
        public string QueueName { get; set; }
        public string Message { get; set; }
    }
}
