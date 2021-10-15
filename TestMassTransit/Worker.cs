using Bogus;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;
using TestMassTransit.Models;

namespace TestMassTransit
{
    public class Worker : BackgroundService
    {
        private readonly IBus _bus;
        private readonly IConfiguration _configuration;
        private readonly Faker _faker;

        private const string MessageQueue = "Message";
        private const string UserQueue = "User";

        public Worker(IBus bus, IConfiguration configuration)
        {
            _bus = bus;
            _faker = new Faker();
            _configuration = configuration;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await Send(MessageQueue, new Message { Text = $"Message: The time is {DateTimeOffset.Now}" });
                await Task.Delay(1500, stoppingToken);

                await Send(UserQueue, new User { Name = $"User: Created new user: {_faker.Name.FullName()}" });
                await Task.Delay(1500, stoppingToken);

                await Send(UserQueue, new UserLocation { Location = $"User Location: Created new user location: {_faker.Random.String2(10)}" });
                await Task.Delay(1500, stoppingToken);
            }
        }

        private async Task Send<T>(string queueName, T message)
        {
            var sendEndPoint = await _bus.GetSendEndpoint(
                new Uri($"{_configuration["ActiveMqUri"]}/{queueName}"));

            await sendEndPoint.Send(message);
        }
    }
}
