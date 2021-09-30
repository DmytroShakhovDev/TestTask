using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RabbitMQ.Client;
using Newtonsoft.Json;
using Microsoft.Extensions.Options;
using System.Text;
using TestProject.WebAPI.Helpers;
using Microsoft.Extensions.Configuration;

namespace TestProject.WebAPI.Services
{
    public class RabbitMQClient
    {
        private readonly IModel _channel;

        public RabbitMQClient(IConfiguration configuration)
        {
            var _config =  new TestProject.WebAPI.Helpers.RabbitMQ();
            configuration.GetSection(nameof(RabbitMQ)).Bind(_config);
            try
            {
                var factory = new ConnectionFactory()
                {
                    HostName = _config.HostName
                    //UserName = _config.UserName,
                    //Password = _config.Password,
                    //Port = _config.Port
                };
                var connection = factory.CreateConnection();
                _channel = connection.CreateModel();
            }
            catch (Exception ex)
            {
                Log.Logger.Error("RabbitMQClient init fail", ex);
            }

        }

        public virtual void PushMessage(string routingKey, string message)
        {
            Log.Logger.Information($"PushMessage,routingKey:{routingKey}");

            _channel.QueueDeclare(queue: "userque",
                                                durable: false,
                                                exclusive: false,
                                                autoDelete: false,
                                                arguments: null);

            var body = Encoding.UTF8.GetBytes(message);

            _channel.BasicPublish(exchange: "",
                                 routingKey: routingKey,
                                 basicProperties: null,
                                 body: body);
        }
    }
}
