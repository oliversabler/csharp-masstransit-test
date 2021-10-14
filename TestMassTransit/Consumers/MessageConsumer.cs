using MassTransit;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using TestMassTransit.Models;

namespace TestMassTransit.Consumers
{
    public class MessageConsumer : IConsumer<Message>, IConsumerMarker
    {
        readonly ILogger<MessageConsumer> _logger;

        public MessageConsumer(ILogger<MessageConsumer> logger)
        {
            _logger = logger;
        }

        public Task Consume(ConsumeContext<Message> context)
        {
            _logger.LogInformation($"Received Text: {context.Message.Text}");

            return Task.CompletedTask;
        }
    }
}
