using System.Text.Json;

namespace Rarakasm.CoolBR.Web.Models
{
    public class Item
    {
        public string Name { get; set; }
        public int Cost { get; set; }
        public int Durability { get; set; }
        public string Class { get; set; }
        
        public override string ToString() => JsonSerializer.Serialize<Item>(this);
    }
}