using DCS.DecisionMakerService.Client.Kafka.Enums;
using DCS.DecisionMakerService.Client.Kafka.Events;
using KafkaFlow;
using KafkaFlow.TypedHandler;
using Loans.Application.AppServices.Contracts.Loans;
using Loans.Application.AppServices.Contracts.Loans.Handlers;
using Loans.Application.AppServices.Contracts.Loans.Models;

namespace Loans.Application.Host.Infrastructure.Kafka.Consumers
{
    /// <summary>
    /// Обработчик событий для результатов расчета решения кредитного контракта из Kafka.
    /// </summary>
    public class CalculateDecisionEventHandler : IMessageHandler<CalculateDecisionEventResult>
    {
        private readonly IProcessLoanContractDecisionHandler _processLoanContractDecisionHandler;
        private readonly ILogger<CalculateDecisionEventHandler> _logger;

        /// <summary>
        /// Инициализирует новый экземпляр класса CalculateDecisionEventHandler.
        /// </summary>
        /// <param name="processLoanContractDecisionHandler">Обрабатчик запрос на сохранение кредитной заявки.</param>
        /// <param name="logger">Logger сообщений.</param>
        public CalculateDecisionEventHandler(IProcessLoanContractDecisionHandler processLoanContractDecisionHandler, ILogger<CalculateDecisionEventHandler> logger)
        {
            _processLoanContractDecisionHandler = processLoanContractDecisionHandler;
            _logger = logger;
        }
        
        /// <summary>
        /// Обрабатывает полученный ответ от сервиса принятия решения по кредиту по средством Кафки.
        /// </summary>
        /// <param name="context">Содержит сообщение и его метаданные.</param>
        /// <param name="message">Ответ от сервиса приянтия решения по кредиту.</param>
        public async Task Handle(IMessageContext context, CalculateDecisionEventResult message)
        {
            _logger.LogInformation("Получение ответа от сервиса кредитной завявки с Id: {ApplicationId}", message.ApplicationId);
            
            var loanContractEventResult = ToLoanContractEventResult(context, message);

            await _processLoanContractDecisionHandler.Handle(loanContractEventResult, CancellationToken.None);
        }

        internal LoanContractEventResult ToLoanContractEventResult(IMessageContext context, CalculateDecisionEventResult message)
        {
            var loanContractEventResult = new LoanContractEventResult
            {
                LoanContractId = message.ApplicationId,
                ClientId = message.ClientId,
                Amount = message.Decision.LoanOffer.CreditAmount,
                InterestRate = message.Decision.LoanOffer.InterestRate,
                LoanDate = context.ConsumerContext.MessageTimestamp,
                LoanTermMonth = message.Decision.LoanOffer.CreditLenMonth
            };
            
            MapStatus(loanContractEventResult, message.Decision.DecisionStatus);

            return loanContractEventResult;
        }

        internal void MapStatus(LoanContractEventResult loanContractEventResult, DecisionStatus status)
        {
            switch (status)
            {
                case DecisionStatus.Unknown:
                    loanContractEventResult.Status = LoanStatus.Unknown;
                    break;
                case DecisionStatus.Approval:
                    loanContractEventResult.Status = LoanStatus.Approved;
                    break;
                case (DecisionStatus.Refuse):
                    loanContractEventResult.Status = LoanStatus.Denied;
                    break;
                case (DecisionStatus.Underwriting):
                    loanContractEventResult.Status = LoanStatus.InProgress;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}