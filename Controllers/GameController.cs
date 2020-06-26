using CoolBR.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CoolBR.Controllers
{
    [ApiController]
    [Route("game")]
    public class GameController : ControllerBase
    {
        private readonly ILogger<GameController> _logger;
        private GameService GameServiceInstance { get; }

        public GameController(
            ILogger<GameController> logger,
            GameService gameServiceInstance)
        {
            _logger = logger;
            GameServiceInstance = gameServiceInstance;
        }

        [HttpGet]
        public int Get()
        {
            return GameServiceInstance.GetCount();
        }
        
        [HttpGet("start")]
        public ActionResult StartTimer()
        {
            GameServiceInstance.StartTimer();
            return Ok();
        }
    }
}