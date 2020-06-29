using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Rarakasm.CoolBR.Web.Misc;

namespace Rarakasm.CoolBR.Web.Services
{
    public class PainterGameService : BackgroundService
    {
        private readonly ILogger<PainterGameService> _logger;

        private readonly ReaderWriterLockSlim _counterLock = new ReaderWriterLockSlim();

        private PainterGame _painterGame;
        
        public PainterGameService(ILogger<PainterGameService> logger)
        {
            _logger = logger;
            _painterGame = PainterGame.CreateGame();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Game service begin working");
            
            stoppingToken.Register(OnCancellation);
            //while (!stoppingToken.IsCancellationRequested) await Task.Yield();
            await Task.CompletedTask;
        }

        private void OnCancellation()
        {
            _logger.LogInformation("Game service cancelled");
        }

        public void SubscribeToGridChange(EventHandler<PainterGame.GridChangedArgs> handler)
        {
            _painterGame.GridChanged += handler;
        }

        public void SetGrid(int row, int col, int num)
        {
            _painterGame?.SetGrid(row, col, num);
        }

        public int[] GetGrid()
        {
            return _painterGame?.GetGrid();
        }

        public int[] GetGridDimensions()
        {
            return _painterGame?.GetGridDimensions();
        }
    }
}