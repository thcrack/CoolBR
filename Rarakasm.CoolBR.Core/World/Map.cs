using System.Text;
using Rarakasm.CoolBR.Core.System;

namespace Rarakasm.CoolBR.Core.World
{
    public class Map
    {
        private int _rows, _cols;
        private Tile[] _tiles;
        private Tileset _tileset;

        public Map(int rows, int cols, Tile[] tiles, Tileset tileset)
        {
            _rows = rows;
            _cols = cols;
            _tiles = tiles;
            _tileset = tileset;
        }

        public bool IsInBound(int row, int col)
        {
            return col >= 0 && col < _cols && row >= 0 && row < _rows;
        }

        public Tile GetTile(int row, int col)
        {
            return _tiles[To1DIndex(row, col)];
        }

        public bool IsTileBlockingMovement(int row, int col)
        {
            return _tileset.IsBlockingMovement(_tiles[To1DIndex(row, col)]);
        }
        
        public bool IsTileBlockingVisibility(int row, int col)
        {
            return _tileset.IsBlockingVisibility(_tiles[To1DIndex(row, col)]);
        }

        public int To1DIndex(int row, int col)
        {
            return col + row * _cols;
        }

        public char[][] GetFormattedTileStrings()
        {
            var result = new char[_rows][];
            for (var i = 0; i < _rows; i++)
            {
                result[i] = new char[_cols];
                for (var j = 0; j < _cols; j++)
                {
                    result[i][j] = GetTile(i, j).Gid == 0 ? '0' : '1';
                }
            }

            return result;
        }

        public int[] GetGidArray()
        {
            var result = new int[_tiles.Length];
            for (var i = 0; i < _tiles.Length; i++)
            {
                result[i] = _tiles[i].Gid;
            }

            return result;
        }

        public Vector2Grid GetDimensions()
        {
            return new Vector2Grid(_rows, _cols);
        }
    }
}