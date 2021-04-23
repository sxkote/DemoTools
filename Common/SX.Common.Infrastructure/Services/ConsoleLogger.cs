using SX.Common.Shared.Contracts;
using System;

namespace SX.Common.Infrastructure.Services
{
    public class ConsoleLogger : ILogger
    {
        private DateTime Now => DateTime.Now;

        public void Error(string message)
        {
            Console.WriteLine($"ERROR: {message}");
        }

        public void Error(Exception exception)
        {
            Console.WriteLine($"ERROR: {exception?.Message}");
        }

        public void Log(string message)
        {
            Console.WriteLine($"LOG: {this.Now}: {message}");
        }

        public void Trace(object data)
        {
            Console.WriteLine($"TRACE: {this.Now}: {data}");
        }
    }
}
