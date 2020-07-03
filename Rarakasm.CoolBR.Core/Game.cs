using System;
using System.Collections.Generic;
using Rarakasm.CoolBR.Core.System;
using Rarakasm.CoolBR.Core.System.FieldOfView;
using Rarakasm.CoolBR.Core.World;

namespace Rarakasm.CoolBR.Core
{
    public class Game
    {
        private Map _map;
        public static Game MakeGame(string mapPath)
        {
            return new Game(MapLoader.LoadMap(mapPath));
        }

        private Game(Map map)
        {
            _map = map;
        }

        public IEnumerable<Vector2Grid> GetVisibleGrids(int row, int col, int maxRange)
        {
            return FOVCalculator.CalculateVisibility(_map, row, col, maxRange);
        }

        public int[] GetMapGidArray()
        {
            return _map.GetGidArray();
        }

        public Vector2Grid GetMapDimensions()
        {
            return _map.GetDimensions();
        }
    }
}