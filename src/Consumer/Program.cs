using System;
using System.Text;
using System.Threading;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Consumer
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory() {HostName = "localhost"};
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "msgKey",
                    durable: false,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);
                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body);
                    Console.WriteLine($" [x] Received message: {message} - Wait {1000 * body.Length}ms ");
                    
                    Thread.Sleep(1000 * body.Length);
                    Console.WriteLine($" [x] Done! ");
                    
                };
                channel.BasicConsume(queue: "msgKey",
                    autoAck: true,
                    consumer: consumer);
                Console.WriteLine("Press any key to exit");
                Console.ReadLine();
            }
        }
    }
}