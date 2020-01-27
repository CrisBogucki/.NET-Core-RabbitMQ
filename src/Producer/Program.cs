using System;
using System.Text;
using RabbitMQ.Client;

namespace Producer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World! I'm Procuder");

            var factory = new ConnectionFactory() {HostName = "localhost"};
            using var connection = factory.CreateConnection();
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(
                    queue: "hellow", 
                    durable: false, 
                    exclusive: false, 
                    autoDelete: false,
                    arguments: null);

                string msg = "Hellow World";
                var body = Encoding.UTF8.GetBytes(msg);

                channel.BasicPublish(
                    exchange: "", 
                    routingKey: "hellow", 
                    basicProperties: null, 
                    body: body);
                
                Console.WriteLine(" [x] Send {0}", msg);
            }
            
            Console.WriteLine("Press enter to exit");
            Console.ReadKey();
        }
    }
}