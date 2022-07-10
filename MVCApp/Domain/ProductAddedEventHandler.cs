using MassTransit;
using MVCApp.Interface;
using MVCApp.Models;
using Polly;

namespace MVCApp.Domain
{
    public  class ProductAddedEventHandler : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ILogger<ProductAddedEventHandler> _logger;
        private CancellationToken _stoppingToken;

        public ProductAddedEventHandler(
            IServiceScopeFactory serviceScopeFactory,
            ILogger<ProductAddedEventHandler> logger)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
            DomainEvent.Register<ProductAdded>(ev => { _ = SendEmailNotification(ev); });
        }

        private async Task SendEmailNotification(ProductAdded ev)
        {
            await using var scope = _serviceScopeFactory.CreateAsyncScope();
            var emailSender = scope.ServiceProvider.GetRequiredService<IEmailSender>();

            Task SendEmailAsync(Product product ,CancellationTokenSource cancelTokenSource)
            {
                return emailSender.SendEmailAsync(product,cancelTokenSource);              
            }
            var policy = Policy
                .Handle<ConnectionException>() //ретраим только ошибки подключения
                .WaitAndRetryAsync(3,
                    retryAttempt => TimeSpan.FromSeconds(Math.Pow(retryAttempt, 2)),
                    (exception, retryAttempt) =>
                    {
                        _logger.LogWarning(
                            exception, "There was an error while sending email. Retrying: {Attempt}", retryAttempt);
                    });
            var result = await policy.ExecuteAndCaptureAsync(SendEmailAsync,cancelTokenSource);
            if (result.Outcome == OutcomeType.Failure)
                _logger.LogError(result.FinalException, "There was an error while sending email");
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _stoppingToken = stoppingToken;
            return Task.CompletedTask;
        }
    }
}
