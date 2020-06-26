using System;
using System.Drawing;

namespace CoolBR.Game
{
    public class BRGame
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
        public static BRGame CreateGame()
        {
            var game = new BRGame();
            return game;
        }

        private BRGame()
        {
            _grid = new int[Rows * Cols];
        }

        public void SetGrid(int row, int col)
        {
            if (row < 0 || row >= Rows 
                || col < 0 || col >= Cols)
            {
                return;
            }
            
            _grid[row * Cols + col] ^= 1;
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