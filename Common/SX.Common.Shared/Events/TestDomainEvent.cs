using SX.Common.Shared.Interfaces;
using System;

namespace SX.Common.Shared.Events
{
    public class TestDomainEvent : IDomainEvent
    {
        public DateTime Date { get; set; }
        public string Message { get; set; }
    }
}
