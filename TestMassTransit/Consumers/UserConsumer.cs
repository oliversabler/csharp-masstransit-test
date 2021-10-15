using MassTransit;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using TestMassTransit.Models;

namespace TestMassTransit.Consumers
{
    public class UserConsumer : IConsumer<User>, IConsumer<UserLocation>
    {
        private readonly ILogger<MessageConsumer> _logger;

        public UserConsumer(ILogger<MessageConsumer> logger)
        {
            _logger = logger;
        }

        public Task Consume(ConsumeContext<User> context)
        {
            _logger.LogInformation($"Received User: {context.Message.Name}");

            return Task.CompletedTask;
        }

        public Task Consume(ConsumeContext<UserLocation> context)
        {
            _logger.LogInformation($"Received User Location: {context.Message.Location}");

            return Task.CompletedTask;
        }
    }
}
