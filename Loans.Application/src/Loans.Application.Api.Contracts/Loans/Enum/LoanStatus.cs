using System.Text.Json.Serialization;

namespace Loans.Application.Api.Contracts.Loans.Enum
{
    /// <summary>
    /// Возможные статусы кредитного договора.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]  
    public enum LoanStatus
    {
        /// <summary>
        /// Статус неизвестен.
        /// </summary>
        Unknown,

        /// <summary>
        /// Кредитный договор находится в процессе обработки.
        /// </summary>
        InProgress,

        /// <summary>
        /// Кредитный договор одобрен.
        /// </summary>
        Approved,

        /// <summary>
        /// Кредитный договор отклонен.
        /// </summary>
        Denied
    }
}