using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SX.Common.Infrastructure.Models;
using SX.Common.Shared.Classes;
using SX.Common.Shared.Events;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SX.Common.Api.Services
{
    public class RabbitMQBackgroundService : BackgroundService
    {
        private readonly RabbitMQConfig _config;
        private IConnection _connection;
        private IModel _channel;

        private string QueueName => _config?.QueueName ?? "";

        public RabbitMQBackgroundService(IOptions<RabbitMQConfig> options)
        {
            _config = options.Value;

            this.CreateConnection();
        }

        //protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        //{
        //    while (!stoppingToken.IsCancellationRequested)
        //    {
        //        CheckConfirmedGracePeriodOrders();

        //        await Task.Delay(_config.ExecutionTimeout, stoppingToken);
        //    }
        //}

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
            //_connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(queue: this.QueueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (ch, ea) =>
            {
                var content = Encoding.UTF8.GetString(ea.Body.ToArray());

                DomainDispatcher.RaiseEvent(new EventBusMessageReceivedEvent()
                {
                    QueueName = this.QueueName,
                    Message = content
                });

                //var model = CommonService.Deserialize<IDomainEvent>(content);
                //DomainDispatcher.RaiseEvent(model);

                _channel.BasicAck(ea.DeliveryTag, false);
            };
            //consumer.Shutdown += OnConsumerShutdown;
            //consumer.Registered += OnConsumerRegistered;
            //consumer.Unregistered += OnConsumerUnregistered;
            //consumer.ConsumerCancelled += OnConsumerCancelled;

            _channel.BasicConsume(this.QueueName, false, consumer);

            return Task.CompletedTask;
        }

        public override void Dispose()
        {
            base.Dispose();

            if (_channel != null)
            {
                _channel.Close();
                _channel = null;
            }

            if (_connection != null)
            {
                _connection.Close();
                _connection = null;
            }
        }
    }
}
