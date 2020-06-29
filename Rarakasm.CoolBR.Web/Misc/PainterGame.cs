using System;

namespace Rarakasm.CoolBR.Web.Misc
{
    public class PainterGame
    {
        private readonly int[] _grid;
        private const int Rows = 20, Cols = 40;
        public event EventHandler<GridChangedArgs> GridChanged;

        public class GridChangedArgs : EventArgs
        {
            public int row, col, id;

            public GridChangedArgs(int row, int col, int id)
            {
                this.row = row;
                this.col = col;
                this.id = id;
            }
        }
        public static PainterGame CreateGame()
        {
            var game = new PainterGame();
            return game;
        }

        private PainterGame()
        {
            _grid = new int[Rows * Cols];
        }

        public void SetGrid(int row, int col, int num)
        {
            if (row < 0 || row >= Rows 
                || col < 0 || col >= Cols)
            {
                return;
            }
            
            _grid[row * Cols + col] = num;
            GridChanged?.Invoke(this,
                new GridChangedArgs(row, col, _grid[row * Cols + col]));
        }

        public int[] GetGrid()
        {
            return _grid;
        }

        public int[] GetGridDimensions()
        {
           return new int[] {Rows, Cols};
        }
    }
}