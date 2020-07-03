using System.Xml.Linq;

namespace Rarakasm.CoolBR.Core.World
{
    public class MapLoader
    {
        public static Map LoadMap(string path)
        {
            var xdoc = XDocument.Load(path);
            var mapEl = xdoc.Element("map");
            var rows = (int) mapEl.Attribute("height");
            var cols = (int) mapEl.Attribute("width");
            var tiles = new Tile[rows * cols];
            // var layers = new List<XElement>(mapEl.Elements("layer"));
            var idx = 0;
            foreach (var t in mapEl.Element("layer").Element("data").Value.Split(','))
            {
                tiles[idx++] = new Tile(int.Parse(t));
            }
            return new Map(rows, cols, tiles, new Tileset(mapEl.Element("tileset")));
        }
    }
}