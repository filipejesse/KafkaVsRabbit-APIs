using Confluent.Kafka;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ConsumerKafka
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var hostBuilder = CreateHostBuilder(args).Build();
            ApplicationRun(hostBuilder);
        }

        private static void ApplicationRun(IHost hostBuilder)
        {

            var config = new ConsumerConfig
            {
                BootstrapServers = "localhost:9092",
                GroupId = "foo",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            using (var consumer = new ConsumerBuilder<Ignore, string>(config).Build())
            {
                var cancellationToken = new CancellationTokenSource();
                Console.CancelKeyPress += (_, e) =>
                {
                    e.Cancel = true; // prevent the process from terminating.
                    cancellationToken.Cancel();
                };
                consumer.Subscribe("kafkaTopic");


                Task.Run(() =>
                {
                    while (true)
                    {
                        var consumeResult = consumer.Consume(cancellationToken.Token);
                        hostBuilder.Services.GetRequiredService<IHandleMessagesHelper>().ReceiveMessage(consumeResult.Message.Value);
                    }
                });

                hostBuilder.Run();
                
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                 .ConfigureServices((_, services) =>
                 {
                     services.AddTransient<IHandleMessagesHelper, HandleMessagesHelper>();
                 })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
