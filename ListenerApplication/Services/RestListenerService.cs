using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace ListenerApplication.Services
{
    public class RestListenerService
    {
        private readonly ILogger _logger;
        ConnectionFactory factory;
        IConnection connection;
        IModel channel;

        public RestListenerService(ILogger<RestListenerService> logger)
        {
            _logger = logger;
            this.factory = new ConnectionFactory()
            {
                Uri = new System.Uri("amqp://guest:guest@rabbitmq:5672/")
            };
            this.connection = factory.CreateConnection();
            this.channel = connection.CreateModel();
        }

        public void Register()
        {
            channel.ExchangeDeclare(exchange: "user", type: "topic");
            var queueName = channel.QueueDeclare().QueueName;

            channel.QueueBind(queue: queueName,
                                exchange: "user",
                                routingKey: "user.created");

            Console.WriteLine(" [*] Waiting for messages. To exit press CTRL+C");

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body;
                var message = Encoding.UTF8.GetString(body);
                var routingKey = ea.RoutingKey;
                
                Console.WriteLine(" [x] Received '{0}':'{1}'",
                                  routingKey,
                                  message);
            };

            channel.BasicConsume(queue: queueName,
                                 autoAck: true,
                                 consumer: consumer);
        }

        public void Deregister()
        {
            this.connection.Close();
        }
    }
}
