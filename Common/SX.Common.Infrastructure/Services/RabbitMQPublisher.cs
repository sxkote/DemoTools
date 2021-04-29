using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using SX.Common.Infrastructure.Models;
using SX.Common.Shared.Contracts;
using SX.Common.Shared.Services;
using System;
using System.Text;

namespace SX.Common.Infrastructure.Services
{
    public class RabbitMQPublisher : IEventBusPublisher
    {
        private readonly RabbitMQConfig _config;
        private IConnection _connection;

        public RabbitMQPublisher(IOptions<RabbitMQConfig> options)
        {
            _config = options.Value;

            this.CreateConnection();
        }

        public void Publish<T>(T data, string queueName = null)
        {
            if (!this.CheckConnection())
                return;

            // define Queue Name
            var queue = String.IsNullOrWhiteSpace(queueName) ? _config.QueueName : queueName;

            using (var channel = _connection.CreateModel())
            {
                channel.QueueDeclare(queue: queue, durable: false, exclusive: false, autoDelete: false, arguments: null);

                var json = CommonService.Serialize(data);
                var body = Encoding.UTF8.GetBytes(json);

                channel.BasicPublish(exchange: "", routingKey: queue, basicProperties: null, body: body);
            }
        }

        private void CreateConnection()
        {
            var factory = new ConnectionFactory
            {
                HostName = _config.Server,
                Port = _config.Port,
                UserName = _config.Login,
                Password = _config.Password,

            };

            _connection = factory.CreateConnection();
        }

        private bool CheckConnection()
        {
            if (_connection == null || !_connection.IsOpen)
                CreateConnection();

            return _connection?.IsOpen ?? false;
        }

        public void Dispose()
        {
            if (_connection != null && _connection.IsOpen)
            {
                _connection.Close();
                _connection = null;
            }
        }
    }
}
