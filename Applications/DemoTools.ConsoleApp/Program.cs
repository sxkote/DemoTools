using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SX.Common.Shared.Events;
using SX.Common.Shared.Services;
using System;
using System.Text;

namespace DemoTools.ConsoleApp
{
    class Program
    {
        public const string TEXT_WELCOME = "Enter command (or 'exit'):";
        public const string TEXT_BYE = "Thanks for using Demo-Tools! Bye!";
        public const string MQ_QUEUE_NAME = "test-queue-1";
        public const string MQ_HOST_NAME = "192.168.1.173";
        public const int MQ_HOST_PORT = 32758;

        static void Main(string[] args)
        {
            // creating message-queue connection and channel
            using (var connection = BuildConnection())
            using (var channel = connection.CreateModel())
            {
                // declaring (creating queue)
                channel.QueueDeclare(queue: MQ_QUEUE_NAME,
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                // building consumer
                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    Console.WriteLine("received: {0}", message);
                };

                int index = 1;
                while (true)
                {
                    Console.WriteLine(TEXT_WELCOME);
                    var command = Console.ReadLine().Trim();

                    if (command.ToLower() == "exit" || command.ToLower() == "quit")
                        break;

                    if (command.ToLower().StartsWith("send"))
                    {
                        var message = BuildMessage(command);

                        var body = Encoding.UTF8.GetBytes(message);

                        channel.BasicPublish(exchange: "",
                                             routingKey: MQ_QUEUE_NAME,
                                             basicProperties: null,
                                             body: body);

                        Console.WriteLine($"Sent: {message}");
                    }
                    else if (command.ToLower() == "receive")
                    {
                        channel.BasicConsume(queue: MQ_QUEUE_NAME,
                                     autoAck: true,
                                     consumer: consumer);
                    }
                }
            }

            Console.WriteLine(TEXT_BYE);
        }

        static public IConnection BuildConnection()
        {
            var factory = new ConnectionFactory() { HostName = MQ_HOST_NAME, Port = MQ_HOST_PORT };
            return factory.CreateConnection();
        }

        static public void SendMessage(string message)
        {
            using (var connection = BuildConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: MQ_QUEUE_NAME,
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: "",
                                     routingKey: MQ_QUEUE_NAME,
                                     basicProperties: null,
                                     body: body);

                Console.WriteLine(" [x] Sent {0}", message);
            }
        }

        static public void ReceiveMessage()
        {
            using (var connection = BuildConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: MQ_QUEUE_NAME,
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    Console.WriteLine(" [x] Received {0}", message);
                };
                channel.BasicConsume(queue: MQ_QUEUE_NAME,
                                     autoAck: true,
                                     consumer: consumer);
            }
        }

        static public string BuildMessage(string command)
        {
            var now = DateTime.Now;
            var message = $"test message: {now}";
            var args = command.Length > 5 ? command.Substring(4).Trim() : "";

            if (args.ToLower().StartsWith("test"))
            {
                var domainEvent = new TestDomainEvent()
                {
                    Date = now,
                    Message = args.Length > 5 ? args.Substring(4).Trim() : ""
                };
                message = CommonService.Serialize(domainEvent);
            }
            else if (!String.IsNullOrWhiteSpace(args))
            {
                message = args;
            }

            return message;
        }
    }
}
