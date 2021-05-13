using RabbitMQ.Client;
using System;
using System.Linq;
using System.Text;

namespace RabbitMQProject.Publisher
{
    class Program
    {
        public enum LogNames 
        {
            Critical = 1,
            Error = 2,
            Warning = 3,
            Info = 4
        }

        static void Main(string[] args)
        {
            var factory = new ConnectionFactory();
            factory.Uri = new Uri("amqps://tfmwqnnj:SoJ7GgKdKrK18y8BgHMtOytN2iVGixbT@fish.rmq.cloudamqp.com/tfmwqnnj");

            using var connection = factory.CreateConnection();
            var channel = connection.CreateModel();

            // channel.QueueDeclare("first-queue", true, false, false);

            channel.ExchangeDeclare("logs-direct", durable: true, type: ExchangeType.Direct);

            Enum.GetNames(typeof(LogNames)).ToList().ForEach(x => {
                var queueName = $"direct-queue-{x}";
                channel.QueueDeclare(queueName, true, false, false);
                var routeKey = $"route-{x}";
                channel.QueueBind(queueName, "logs-direct", routeKey);
            });


            Enumerable.Range(1, 50).ToList().ForEach(x => {

                LogNames log = (LogNames)new Random().Next(1, 5);

                string message = $"Log Type:  {log}";

                var messageBody = Encoding.UTF8.GetBytes(message);

                var routeKey = $"route-{log}";

                channel.BasicPublish("logs-direct", routeKey, null, messageBody);

                Console.WriteLine("Log has been sended: {0}", message);

            });

            Console.ReadLine();
        }
    }
}
