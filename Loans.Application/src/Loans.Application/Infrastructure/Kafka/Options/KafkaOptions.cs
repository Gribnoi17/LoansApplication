namespace Loans.Application.Host.Infrastructure.Kafka.Options
{
    /// <summary>
    /// Класс конфигурации Kafka.
    /// </summary>
    public class KafkaOptions
    {
        public const string Section = "KafkaConfigurations";
        
        /// <summary>
        /// Список серверов Kafka.
        /// </summary>
        public string[] Servers { get; init; } = Array.Empty<string>();
        
        /// <summary>
        /// Консьюмер группа Kafka.
        /// </summary>
        public string? ConsumerGroup { get; init; }
    }
}