using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;


namespace MQReciever
{
    public static class Program
    {
        public static void Main()
        {
            //Console.WriteLine("Hello World!");

            string connectionString = "";
            connectionString = System.Configuration.ConfigurationManager.
                ConnectionStrings["rabbitmq"].ConnectionString;
            
            Console.WriteLine(connectionString);
            var factory = new ConnectionFactory() { HostName= connectionString };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "userque",
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    Console.WriteLine(" [x] Received {0}", message);
                };
                channel.BasicConsume(queue: "userque",
                                     autoAck: true,
                                     consumer: consumer);

                Console.WriteLine(" Press [enter] to exit.");
                Console.ReadLine();
            }

        }
    }
}
