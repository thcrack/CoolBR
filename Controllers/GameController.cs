using System.Data;
using CoolBR.Game;
using CoolBR.Services;
using CoolBR.Services.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace CoolBR.Controllers
{
    [ApiController]
    [Route("game")]
    public class GameController : ControllerBase
    {
        private readonly ILogger<GameController> _logger;
        private readonly IHubContext<GameHub> GameHubContext;
        private GameService GameServiceInstance { get; }

        public GameController(
            ILogger<GameController> logger,
            GameService gameServiceInstance,
            IHubContext<GameHub> gameHubContext)
        {
            _logger = logger;
            GameServiceInstance = gameServiceInstance;
            GameServiceInstance.SubscribeToGridChange(OnGridChanged);
            GameHubContext = gameHubContext;
        }

        private void OnGridChanged(object o, BRGame.GridChangedArgs e)
        {
            GameHubContext.Clients.All.SendAsync(
                "GridChanged",
                e.row, e.col, e.id
                );
        }

        public struct GridInfo
        {
            public int Rows { get; set; }
            public int Cols { get; set; }
            public int[] Data { get; set; }
        }

        [HttpGet]
        public GridInfo Get()
        {
            var dims = GameServiceInstance.GetGridDimensions();
            return new GridInfo
            {
                Rows = dims[0],
                Cols = dims[1],
                Data = GameServiceInstance.GetGrid()
            };
        }
        
        [HttpGet("set")]
        public ActionResult SetGrid(
            [FromQuery] int row,
            [FromQuery] int col
            )
        {
            GameServiceInstance.SetGrid(row, col);
            return Ok();
        }
    }
}