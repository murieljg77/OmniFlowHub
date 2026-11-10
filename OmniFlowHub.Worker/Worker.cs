using OmniFlowHub.Application.Interfaces;

namespace OmniFlowHub.Worker
{
    public class Worker : BackgroundService
    {
        readonly IServiceProvider _serviceProvider;
        readonly ILogger<Worker> _logger;
        readonly TimeSpan _interval = TimeSpan.FromSeconds(15);

        public Worker(IServiceProvider serviceProvider, ILogger<Worker> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Worker iniciado.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = _serviceProvider.CreateScope();
                    var processingService = scope.ServiceProvider.GetRequiredService<IDocumentProcessingService>();

                    await processingService.ProcessPendingDocumentsAsync(stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error ejecutando el procesamiento en segundo plano.");
                }

                await Task.Delay(_interval, stoppingToken);
            }
        }
    }
}