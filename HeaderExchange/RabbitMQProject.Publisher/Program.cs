using RabbitMQ.Client;
using System;
using System.Collections.Generic;
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

            channel.ExchangeDeclare("header-exchange", durable: true, type: ExchangeType.Headers);

            Dictionary<string, object> headers = new Dictionary<string, object>();
            headers.Add("format", "pdf");
            headers.Add("shape", "a4");

            var properties = channel.CreateBasicProperties();
            properties.Headers = headers;

            channel.BasicPublish("header-exchange", string.Empty, properties, Encoding.UTF8.GetBytes("Header Message!"));
            Console.WriteLine("Message has been sended.");

            Console.ReadLine();
        }
    }
}
