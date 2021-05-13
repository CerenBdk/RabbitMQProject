using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace RabbitMqProject.Subscriber
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory();
            factory.Uri = new Uri("amqps://tfmwqnnj:SoJ7GgKdKrK18y8BgHMtOytN2iVGixbT@fish.rmq.cloudamqp.com/tfmwqnnj");

            using var connection = factory.CreateConnection();
            var channel = connection.CreateModel();

            channel.BasicQos(0, 1, false);

            var consumer = new EventingBasicConsumer(channel);

            var queueName = channel.QueueDeclare().QueueName;

            Dictionary<string, object> headers = new Dictionary<string, object>();
            headers.Add("format", "pdf");
            headers.Add("shape", "a4");
            headers.Add("x-match", "any");

            channel.QueueBind(queueName, "header-exchange", string.Empty, headers);

            channel.BasicConsume(queueName, false, consumer);

            Console.WriteLine("Listening..");
            consumer.Received += (object sender, BasicDeliverEventArgs e) =>
            {

                var message = Encoding.UTF8.GetString(e.Body.ToArray());

                Thread.Sleep(1500);
                Console.WriteLine("Incoming message: " + message);

                channel.BasicAck(e.DeliveryTag, false);
            };
            Console.ReadLine();

        }
    }
}
