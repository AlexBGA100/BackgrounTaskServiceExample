using Application.Abstractions.Interfaces;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Infrastructure.Background
{
    public class QueuedHostedService : BackgroundService
    {
        private readonly IBackgroundTaskQueue _taskQueue;
        private readonly ILogger<QueuedHostedService> _logger;
        private readonly int _concurrentTaskLimit;
        /// <summary>
        /// Сonstructor.
        /// </summary>
        /// <param name="taskQueue"> Queue of background tasks.</param>
        /// <param name="logger">Logger.</param>
        /// <param name="concurrentTaskLimit">The maximum number of simultaneous tasks (by default 10).</param>
        public QueuedHostedService(IBackgroundTaskQueue taskQueue, 
                                   ILogger<QueuedHostedService> logger, 
                                   int concurrentTaskLimit = 10)
        {
            _taskQueue = taskQueue;
            _logger = logger;
            _concurrentTaskLimit = concurrentTaskLimit;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("QueuedHostedService run.");

            var workers = new List<Task>();
            for (int i = 0; i < _concurrentTaskLimit; i++)
            {
                workers.Add(Task.Run(() => ProcessQueueAsync(stoppingToken), stoppingToken));
            }

            await Task.WhenAll(workers);
        }

        private async Task ProcessQueueAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var workItem = await _taskQueue.DequeueAsync(stoppingToken);
                    await workItem(stoppingToken);
                }
                catch (OperationCanceledException)
                {
                    break;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error Queue task");
                }
            }
        }
    }
}
