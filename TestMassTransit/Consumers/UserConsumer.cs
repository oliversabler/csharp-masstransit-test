using MassTransit;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using TestMassTransit.Models;

namespace TestMassTransit.Consumers
{
    public class UserConsumer : IConsumer<User>
    {
        readonly ILogger<MessageConsumer> _logger;

        public UserConsumer(ILogger<MessageConsumer> logger)
        {
            _logger = logger;
        }

        public Task Consume(ConsumeContext<User> context)
        {
            _logger.LogInformation($"Received User: {context.Message.Name}");

            return Task.CompletedTask;
        }
    }
}
