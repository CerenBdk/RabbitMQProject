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

            channel.ExchangeDeclare("logs-topic", durable: true, type: ExchangeType.Topic);

            Enumerable.Range(1, 50).ToList().ForEach(x =>
            {
                LogNames log1 = (LogNames)new Random().Next(1, 5);
                LogNames log2 = (LogNames)new Random().Next(1, 5);
                LogNames log3 = (LogNames)new Random().Next(1, 5);

                string message = $"Log Type:  {log1}-{log2}-{log3}";

                var messageBody = Encoding.UTF8.GetBytes(message);

                var routeKey = $"{log1}.{log2}.{log3}";

                channel.BasicPublish("logs-topic", routeKey, null, messageBody);

                Console.WriteLine("Log has been sended: {0}", message);

            });

            Console.ReadLine();
        }
    }
}
