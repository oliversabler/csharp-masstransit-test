using MassTransit;
using MassTransit.ActiveMqTransport.Configurators;
using MassTransit.Definition;
using MassTransit.Registration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace TestMassTransit
{
    public class Program
    {
        public static IConfiguration Configuration { get; set; }

        public static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json");

            Configuration = builder.Build();

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddMassTransit(c =>
                    {
                        c.AddConsumers(typeof(IAssemblyMarker).Assembly);

                        c.UsingActiveMq((context, config) =>
                        {
                            config.Host(new ConfigurationHostSettings(new Uri(Configuration["ActiveMqUri"])));

                            config.ConfigureEndpoints(
                                context,
                                DefaultEndpointNameFormatter.Instance);
                        });
                    });

                    services.AddMassTransitHostedService(true);

                    services.AddHostedService<Worker>();
                });
    }
}
