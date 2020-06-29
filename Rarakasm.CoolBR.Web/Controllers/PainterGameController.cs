using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Rarakasm.CoolBR.Web.Misc;
using Rarakasm.CoolBR.Web.Services;
using Rarakasm.CoolBR.Web.Services.Hubs;

namespace Rarakasm.CoolBR.Web.Controllers
{
    [ApiController]
    [Route("api/painter")]
    public class PainterGameController : ControllerBase
    {
        private readonly ILogger<PainterGameController> _logger;
        private readonly IHubContext<PainterGameHub> GameHubContext;
        private PainterGameService PainterGameServiceInstance { get; }

        public PainterGameController(
            ILogger<PainterGameController> logger,
            PainterGameService painterGameServiceInstance,
            IHubContext<PainterGameHub> gameHubContext)
        {
            _logger = logger;
            PainterGameServiceInstance = painterGameServiceInstance;
            PainterGameServiceInstance.SubscribeToGridChange(OnGridChanged);
            GameHubContext = gameHubContext;
        }

        private void OnGridChanged(object o, PainterGame.GridChangedArgs e)
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
            var dims = PainterGameServiceInstance.GetGridDimensions();
            return new GridInfo
            {
                Rows = dims[0],
                Cols = dims[1],
                Data = PainterGameServiceInstance.GetGrid()
            };
        }
        
        [HttpPut("set")]
        public ActionResult SetGrid(
            [FromQuery] int row,
            [FromQuery] int col,
            [FromQuery] int num
            )
        {
            PainterGameServiceInstance.SetGrid(row, col, num);
            return Ok();
        }
    }
}