using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading;

namespace RabbitMQProject.Subscriber
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory();
            factory.Uri = new Uri("amqps://tfmwqnnj:SoJ7GgKdKrK18y8BgHMtOytN2iVGixbT@fish.rmq.cloudamqp.com/tfmwqnnj");

            using var connection = factory.CreateConnection();
            var channel = connection.CreateModel();

            //channel.QueueDeclare("first-queue", true, false, false);

            // var randomQueueName = "log-database-save"; 

            var randomQueueName = channel.QueueDeclare().QueueName;

            //channel.QueueDeclare(randomQueueName, true, false, false);

            channel.QueueBind(randomQueueName, "logs-fanout", "", null);

            channel.BasicQos(0, 1, false);

            var consumer = new EventingBasicConsumer(channel);

            channel.BasicConsume(randomQueueName, false, consumer);

            Console.WriteLine("Log Listening");
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
