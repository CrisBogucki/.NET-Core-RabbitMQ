using System;
using System.Text;
using RabbitMQ.Client;

namespace Producer
{
    class Program
    {
        static void Main(string[] args)
        {
            app();
        }

        private static void app()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: "msgKey",
                        durable: false,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null);

                    Console.Write("Enter message: ");
                    string msg = Console.ReadLine();
                    var msgBody = Encoding.UTF8.GetBytes(msg);
                    channel.BasicPublish(exchange: "",
                        routingKey: "msgKey",
                        basicProperties: null,
                        body: msgBody);
                    Console.WriteLine($" [x] Send message: {msg}");
                }
            }
            
            Console.Write("Exit [T/N]");
            var isEnd = Console.ReadLine();
            if(!(isEnd != null && isEnd.ToLower() == "t")) 
                app();
        }
    }
}