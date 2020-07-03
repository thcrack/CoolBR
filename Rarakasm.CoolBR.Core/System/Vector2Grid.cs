namespace Rarakasm.CoolBR.Core.System
{
    public struct Vector2Grid
    {
        public int Row { get; set;  }
        public int Col { get; set; }

        public Vector2Grid(int row, int col)
        {
            Row = row;
            Col = col;
        }
    }
}