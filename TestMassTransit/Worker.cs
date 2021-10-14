using Bogus;
using MassTransit;
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
        private readonly Faker _faker;

        public Worker(IBus bus)
        {
            _bus = bus;
            _faker = new Faker();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await _bus.Publish(new Message { Text = $"Message: The time is {DateTimeOffset.Now}" });
                await Task.Delay(2500, stoppingToken);

                await _bus.Publish(new User { Name = $"User: Created new user: {_faker.Name.FullName()}" });
                await Task.Delay(2500, stoppingToken);
            }
        }
    }
}
