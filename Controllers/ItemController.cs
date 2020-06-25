using System;
using System.Collections.Generic;
using CoolBR.Models;
using CoolBR.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CoolBR.Controllers
{
    [ApiController]
    [Route("item")]
    public class ItemController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;
        private JsonItemService ItemService { get; }

        public ItemController(
            ILogger<WeatherForecastController> logger,
            JsonItemService itemService)
        {
            _logger = logger;
            ItemService = itemService;
        }

        [HttpGet("all")]
        public IEnumerable<Item> GetAll()
        {
            return ItemService.GetItems();
        }
        
        [HttpGet]
        public Item Get([FromQuery] int idx)
        {
            return !(ItemService.GetItems() is Item[] items) || items.Length <= idx ? new Item() : items[idx];
        }
    }
}