using RabbitMQ.Client;
using System;
using System.Linq;
using System.Text;

namespace RabbitMQProject.Publisher
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory();
            factory.Uri = new Uri("amqps://tfmwqnnj:SoJ7GgKdKrK18y8BgHMtOytN2iVGixbT@fish.rmq.cloudamqp.com/tfmwqnnj");

            using var connection = factory.CreateConnection();
            var channel = connection.CreateModel();

            // channel.QueueDeclare("first-queue", true, false, false);

            channel.ExchangeDeclare("logs-fanout", durable: true, type: ExchangeType.Fanout);

            Enumerable.Range(1, 50).ToList().ForEach(x => {

                string message = $"Log {x}";

                var messageBody = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish("logs-fanout", " ", null, messageBody);

                Console.WriteLine("Message has been sended: {0}", message);

            });
            
            Console.ReadLine();
        }
    }
}
