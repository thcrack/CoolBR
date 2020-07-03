using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Rarakasm.CoolBR.Core;
using Rarakasm.CoolBR.Core.System;

namespace Rarakasm.CoolBR.Web.Services
{
    public class CoolBRGameService : BackgroundService
    {
        private Game _game;
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _game = Game.MakeGame(
                Path.GetFullPath(
                Path.Combine(@"..\GameAssets",
                    @"Maps\TestMap.tmx")
            ));
            await Task.CompletedTask;
        }

        public IEnumerable<Vector2Grid> GetVisibleGrids(int row, int col, int maxRange)
        {
            return _game.GetVisibleGrids(row, col, maxRange);
        }

        public IEnumerable<int> GetGameMapGidArray()
        {
            return _game.GetMapGidArray();
        }

        public Vector2Grid GetGameMapDimensions()
        {
            return _game.GetMapDimensions();
        }
    }
}