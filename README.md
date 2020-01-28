# .NET Core working with RabbitMQ

![](https://www.wykop.pl/cdn/c3201142/comment_M7Mgp7H0Uy5CxHd2uUXLG4wWuORmi30G.jpg)
1. Download and run RabbitMQ from docker 
`docker run -d -p 15672:15672 -p 5672:5672 -p 5671:5671 --hostname rabbitmq-host --name rabbitmq-container rabbitmq` 
2. Producer site send message to queue
    ```
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

            string msg = "Hellow World";
            var body = Encoding.UTF8.GetBytes(msg);

            channel.BasicPublish(exchange: "",
                routingKey: "first",
                basicProperties: null,
                body: body); 
            
            Console.WriteLine(" [x] Send {0}", msg);
        }
    ```
   
3. Consumer site receive message from queue
    ```
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

    ```
   
### Release

* [x] Basic queue - send and receive message 
