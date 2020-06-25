using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using CoolBR.Models;
using Microsoft.AspNetCore.Hosting;

namespace CoolBR.Services
{
    public class JsonItemService
    {
        public JsonItemService(IWebHostEnvironment webHostEnvironment)
        {
            WebHostEnvironment = webHostEnvironment;
        }

        public IWebHostEnvironment WebHostEnvironment { get; }

        private string JsonFileName => Path.Combine(WebHostEnvironment.WebRootPath, "placeholders", "items.json");

        public IEnumerable<Item> GetItems()
        {
            using var jsonFileReader = File.OpenText(JsonFileName);
            return JsonSerializer.Deserialize<Item[]>(jsonFileReader.ReadToEnd(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
        }
    }
}