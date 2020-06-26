using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Timer = System.Timers.Timer;

namespace CoolBR.Services
{
    public class GameService : BackgroundService
    {
        private readonly ILogger<GameService> _logger;

        private readonly ReaderWriterLockSlim _counterLock = new ReaderWriterLockSlim();
        private int _counter = 0;
        private Timer _countTimer;
        
        public GameService(ILogger<GameService> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Game service begin working");
            stoppingToken.Register(() =>
            {
                _countTimer?.Stop();
                _countTimer?.Close();
            });
            while (!stoppingToken.IsCancellationRequested) await Task.Yield();
            _logger.LogDebug("Game service is cancelled");
        }

        public void StartTimer()
        {
            _countTimer?.Stop();
            _countTimer?.Close();
            _countTimer = new Timer(200)
            {
                AutoReset = true
            };
            
            _countTimer.Elapsed += (context, e) =>
            {
                _counterLock.EnterWriteLock();
                _counter++;
                _counterLock.ExitWriteLock();
            };
            
            _countTimer.Start();
        }

        public int GetCount()
        {
            _counterLock.EnterReadLock();
            var retVal = _counter;
            _counterLock.ExitReadLock();
            return retVal;
        }
    }
}