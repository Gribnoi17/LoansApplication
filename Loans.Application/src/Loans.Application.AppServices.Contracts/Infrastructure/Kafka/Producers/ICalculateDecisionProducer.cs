using DCS.DecisionMakerService.Client.Kafka.Events;

namespace Loans.Application.AppServices.Contracts.Infrastructure.Kafka.Producers
{
    /// <summary>
    /// Интерфейс для отправки событий CalculateDecisionEvent в Kafka.
    /// </summary>
    public interface ICalculateDecisionProducer
    {
        /// <summary>
        /// Отправляет событие CalculateDecisionEvent в Kafka.
        /// </summary>
        /// <param name="eventMessage">Событие CalculateDecisionEvent для отправки.</param>
        /// <returns>Задача, представляющая асинхронную отправку события.</returns>
        Task SendCalculateDecisionEvent(CalculateDecisionEvent eventMessage);
    }
}