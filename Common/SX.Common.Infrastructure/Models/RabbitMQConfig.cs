using SX.Common.Shared.Models;
using System;

namespace SX.Common.Infrastructure.Models
{
    public class RabbitMQConfig : ServerConfig
    {
        public const string CONFIG_NAME = "RabbitMQConfig";

        public const int DEFAULT_EXECUTION_TIMEOUT = 2 * 60 * 1000;

        public string QueueName { get; set; } = "";
        public int ExecutionTimeout { get; set; } = DEFAULT_EXECUTION_TIMEOUT;

        public override int DefaultPort => 5672;

        public RabbitMQConfig() { }

        public RabbitMQConfig(string connectionString)
            : base(connectionString) { }

        protected override void ParseConnectionStringParams(ParamValueCollection parametres)
        {
            if (parametres == null)
                return;

            base.ParseConnectionStringParams(parametres);

            this.QueueName = parametres.GetValue("QueueName") ?? "";

            if (parametres.Contains("ExecutionTimeout"))
            {
                var value = parametres.GetValue("ExecutionTimeout");
                this.ExecutionTimeout = String.IsNullOrWhiteSpace(value) ? DEFAULT_EXECUTION_TIMEOUT : Int32.Parse(value);
            }

        }
    }
}
