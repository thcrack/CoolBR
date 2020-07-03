using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Rarakasm.CoolBR.Core.System;
using Rarakasm.CoolBR.Web.Models.Game;
using Rarakasm.CoolBR.Web.Services;

namespace Rarakasm.CoolBR.Web.Controllers
{
    [ApiController]
    [Route("api/game")]
    public class CoolBRGameController
    {
        private readonly ILogger<CoolBRGameController> _logger;
        private readonly CoolBRGameService _gameService;
        
        public CoolBRGameController(
            ILogger<CoolBRGameController> logger,
            CoolBRGameService gameService)
        {
            _logger = logger;
            _gameService = gameService;
        }

        [HttpGet("mapgids")]
        public StaticMapInfo GetGameMapInfo()
        {
            var dims = _gameService.GetGameMapDimensions();
            return new StaticMapInfo(dims.Row, dims.Col,
                _gameService.GetGameMapGidArray());
        }

        [HttpGet("visiblegrids")]
        public IEnumerable<int> GetVisibleGrids(
            [FromQuery] int row, 
            [FromQuery] int col, 
            [FromQuery] int maxRange)
        {
            var grids = _gameService.GetVisibleGrids(row, col, maxRange);
            var result = new List<int>();
            foreach (var grid in grids)
            {
                result.Add(grid.Row);
                result.Add(grid.Col);
            }
            return result;
        }
    }
}