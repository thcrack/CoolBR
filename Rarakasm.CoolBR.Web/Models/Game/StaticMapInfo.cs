using System.Collections.Generic;

namespace Rarakasm.CoolBR.Web.Models.Game
{
    public class StaticMapInfo
    {
        public StaticMapInfo(int rows, int cols, IEnumerable<int> gids)
        {
            Rows = rows;
            Cols = cols;
            Gids = gids;
        }

        public int Rows { get; set; }
        public int Cols { get; set; }
        public IEnumerable<int> Gids { get; set; }
    }
}