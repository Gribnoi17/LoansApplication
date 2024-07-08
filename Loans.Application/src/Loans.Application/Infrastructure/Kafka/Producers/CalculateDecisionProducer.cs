using DCS.DecisionMakerService.Client.Kafka.Events;
using KafkaFlow;
using Loans.Application.AppServices.Contracts.Infrastructure.Kafka.Producers;

namespace Loans.Application.Host.Infrastructure.Kafka.Producers
{
    /// <summary>
    /// Реализация интерфейса <see cref="ICalculateDecisionProducer"/> для отправки событий CalculateDecisionEvent в Kafka.
    /// </summary>
    public class CalculateDecisionProducer : ICalculateDecisionProducer
    {
        private readonly IMessageProducer<CalculateDecisionProducer> _producer;
        private readonly ILogger<CalculateDecisionProducer> _logger;

        /// <summary>
        /// Конструктор класса <see cref="CalculateDecisionProducer"/>.
        /// </summary>
        /// <param name="producer">Продюсер сообщений, используемый для отправки событий.</param>
        /// <param name="logger">Logger сообщений.</param>
        public CalculateDecisionProducer(IMessageProducer<CalculateDecisionProducer> producer, ILogger<CalculateDecisionProducer> logger)
        {
            _producer = producer;
            _logger = logger;
        }

        public async Task SendCalculateDecisionEvent(CalculateDecisionEvent eventMessage)
        {
            _logger.LogInformation("Отправка кредитной заявки с Id: {ApplicationId} в сервис принятия решений", eventMessage.ApplicationId);
            
            await _producer.ProduceAsync(
                messageKey: eventMessage.ClientId.ToString(),
                messageValue: eventMessage);
        }
    }
}