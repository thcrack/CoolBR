using System;
using System.Xml.Linq;

namespace Rarakasm.CoolBR.Core.World
{
    [Flags]
    public enum TileCollisionFlag
    {
        None = 0b00000000,
        BlockMovement = 0b00000001,
        BlockVisibility = 0b00000010,
        BlockAll = 0b00000011
    }
    
    public class Tileset
    {
        private int _firstGid, _tileCnt;
        private string _tileImgPath;
        private TileCollisionFlag[] _collisionFlags;

        public Tileset(XElement tilesetEl)
        {
            _tileCnt = (int) tilesetEl.Attribute("tilecount");
            _firstGid = (int) tilesetEl.Attribute("firstgid");
            _collisionFlags = new TileCollisionFlag[_tileCnt];
            _tileImgPath = (string) tilesetEl.Element("image").Attribute("source");
            foreach (var tileEl in tilesetEl.Elements("tile"))
            {
                var id = (int) tileEl.Attribute("id");
                foreach (var propEl in tileEl.Element("properties").Elements("property"))
                {
                    switch ((string)propEl.Attribute("name"))
                    {
                        case "blockMovement":
                            _collisionFlags[id] |= TileCollisionFlag.BlockMovement;
                            break;
                        case "blockVisibility":
                            _collisionFlags[id] |= TileCollisionFlag.BlockVisibility;
                            break;
                    }
                }
            }
        }
        
        public bool IsBlockingMovement(Tile tile)
        {
            return tile.Gid != 0 && _collisionFlags[tile.Gid - _firstGid].HasFlag(TileCollisionFlag.BlockMovement);
        }

        public bool IsBlockingVisibility(Tile tile)
        {
            return tile.Gid != 0 && _collisionFlags[tile.Gid - _firstGid].HasFlag(TileCollisionFlag.BlockVisibility);
        }
    }
}