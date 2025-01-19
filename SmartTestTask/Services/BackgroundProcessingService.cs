using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SmartTestTask.Services
{
    public class BackgroundProcessingService : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                // Simulate background processing
                await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
                Console.WriteLine("Background processing executed.");
            }
        }
    }

}
