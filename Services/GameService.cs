using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using CoolBR.Game;

namespace CoolBR.Services
{
    public class GameService : BackgroundService
    {
        private readonly ILogger<GameService> _logger;

        private readonly ReaderWriterLockSlim _counterLock = new ReaderWriterLockSlim();

        private BRGame _brGame;
        
        public GameService(ILogger<GameService> logger)
        {
            _logger = logger;
            _brGame = BRGame.CreateGame();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Game service begin working");
            
            stoppingToken.Register(OnCancellation);
            while (!stoppingToken.IsCancellationRequested) await Task.Yield();
        }

        private void OnCancellation()
        {
            _logger.LogInformation("Game service cancelled");
        }

        public void SubscribeToGridChange(EventHandler<BRGame.GridChangedArgs> handler)
        {
            _brGame.GridChanged += handler;
        }

        public void SetGrid(int row, int col)
        {
            _brGame?.SetGrid(row, col);
        }

        public int[] GetGrid()
        {
            return _brGame?.GetGrid();
        }

        public int[] GetGridDimensions()
        {
            return _brGame?.GetGridDimensions();
        }
    }
}