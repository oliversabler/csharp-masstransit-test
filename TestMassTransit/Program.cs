using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TestMassTransit.Consumers;

namespace TestMassTransit
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddMassTransit(c =>
                    {
                        c.AddConsumers(typeof(IConsumerMarker).Assembly);

                        c.UsingInMemory((context, config) =>
                        {
                            config.ConfigureEndpoints(context);

                            // How to refactor, is it possible?
                            // ReceiveEndpoint subscribes to a queue.
                            config.ReceiveEndpoint("message-queue", c
                                => c.ConfigureConsumer<MessageConsumer>(context));

                            config.ReceiveEndpoint("user-queue", c
                                => c.ConfigureConsumer<UserConsumer>(context));
                        });
                    });

                    services.AddMassTransitHostedService(true);

                    services.AddHostedService<Worker>();
                });
    }
}
