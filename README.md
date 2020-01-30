# .NET Core working with RabbitMQ

![](https://www.wykop.pl/cdn/c3201142/comment_M7Mgp7H0Uy5CxHd2uUXLG4wWuORmi30G.jpg)
1. Download and run RabbitMQ from docker 
`docker run -d -p 15672:15672 -p 5672:5672 -p 5671:5671 --hostname rabbitmq-host --name rabbitmq-container rabbitmq` 
2. Producer site send message to queue
    ```
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
    ```
   
3. Consumer site receive message from queue
    ```
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

    ```
   
### Release

* [x] Basic queue - send and receive message 