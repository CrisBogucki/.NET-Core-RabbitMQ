using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Consumer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World! I'm Consumer");
            
            var factory = new ConnectionFactory() {HostName = "localhost"};
            using var connection = factory.CreateConnection();
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(
                    queue: "first", 
                    durable: false, 
                    exclusive: false, 
                    autoDelete: false, 
                    arguments: null);
                
                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, obj) =>
                {
                    var body = obj.Body;
                    var msg = Encoding.UTF8.GetString(body);
                    Console.WriteLine(" [x] Received {0}", msg);
                };
                
                channel.BasicConsume(
                    queue: "first", 
                    autoAck: true, 
                    consumer: consumer);
                
            }
            
            Console.WriteLine("Press enter to exit");
            Console.ReadKey();
            
        }
    }
}