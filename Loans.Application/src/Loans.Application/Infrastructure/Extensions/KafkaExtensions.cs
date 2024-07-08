using DCS.DecisionMakerService.Client.Kafka.Events;
using DCS.Platform.Kafka.Abstractions.Helpers;
using KafkaFlow;
using KafkaFlow.Serializer;
using KafkaFlow.TypedHandler;
using Loans.Application.AppServices.Contracts.Infrastructure.Kafka.Producers;
using Loans.Application.Host.Infrastructure.Kafka.Consumers;
using Loans.Application.Host.Infrastructure.Kafka.Options;
using Loans.Application.Host.Infrastructure.Kafka.Producers;

namespace Loans.Application.Host.Infrastructure.Extensions
{
    /// <summary>
    /// Расширения для конфигурации Kafka.
    /// </summary>
    public static class KafkaExtensions
    {
        /// <summary>
        /// Регистрирует Kafka с указанным значением из конфигурации.
        /// </summary>
        /// <param name="services">Коллекция сервисов для регистрации Kafka.</param>
        /// <param name="configuration">Конфигурация, содержащая наименование службы.</param>
        /// <returns>Обновленную коллекцию сервисов.</returns>
        public static IServiceCollection AddKafkaService(this IServiceCollection services, IConfiguration configuration)
        {
            var kafkaOptions = configuration.GetSection(KafkaOptions.Section).Get<KafkaOptions>();
            
            if (kafkaOptions == null)
            {
                throw new NullReferenceException("kafkaOptions is null");
            }

            services.AddKafka(kafka => kafka
                .UseMicrosoftLog()
                .AddCluster(cluster => cluster
                    .WithBrokers(kafkaOptions.Servers)
                    .AddProducer<CalculateDecisionProducer>(builder =>
                    {
                        builder.AddMiddlewares(middlewares => middlewares.AddSerializer<JsonCoreSerializer>());
                        builder.DefaultTopic(KafkaHelpers.GetTopic(typeof(CalculateDecisionEvent)));
                    })
                    .AddConsumer(consumer => consumer
                        .Topic(KafkaHelpers.GetTopic(typeof(CalculateDecisionEventResult)))
                        .WithGroupId(kafkaOptions.ConsumerGroup)
                        .WithBufferSize(100)
                        .WithWorkersCount(10)
                        .AddMiddlewares(middlewares => middlewares
                            .AddSerializer<JsonCoreSerializer>()
                            .AddTypedHandlers(handlers => handlers
                                .WithHandlerLifetime(InstanceLifetime.Scoped)
                                .AddHandler<CalculateDecisionEventHandler>()
                                .WhenNoHandlerFound(context =>
                                    Console.WriteLine("Message not handled > Partition: {0} | Offset: {1}",
                                        context.ConsumerContext.Partition.ToString(),
                                        context.ConsumerContext.Offset.ToString())
                                )
                            )
                        )
                    )
                )
            );

            services.AddScoped<ICalculateDecisionProducer, CalculateDecisionProducer>();

            return services;
        }
        
        /// <summary>
        /// Расширение для добавления Kafka шины в приложение.
        /// </summary>
        /// <param name="app">Экземпляр <see cref="IApplicationBuilder"/>.</param>
        /// <param name="lifetime">Интерфейс <see cref="IHostApplicationLifetime"/> для управления временем жизни приложения.</param>
        public static IApplicationBuilder UseKafkaBus(this IApplicationBuilder app, IHostApplicationLifetime lifetime)
        {
            var kafkaBus = app.ApplicationServices.CreateKafkaBus();
            lifetime.ApplicationStarted.Register(() => kafkaBus.StartAsync(lifetime.ApplicationStopped));

            return app;
        }
    }
}