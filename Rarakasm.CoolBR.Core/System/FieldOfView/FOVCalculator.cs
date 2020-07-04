using System;
using System.Collections.Generic;
using System.Numerics;
using Rarakasm.CoolBR.Core.World;

namespace Rarakasm.CoolBR.Core.System.FieldOfView
{
    public class FOVCalculator
    {
        private const double InShadowRatio = .4;

        // row * oct[i][0] + col * oct[i][1]
        // row * oct[i][2] + col * oct[i][3]
        private static readonly int[,] Oct = 
        {
            {  1,  0,  0,  1 }, {  0,  1,  1,  0 },
            {  0, -1,  1,  0 }, { -1,  0,  0,  1 },
            { -1,  0,  0, -1 }, {  0, -1, -1,  0 },
            {  0,  1, -1,  0 }, {  1,  0,  0, -1 }
        };

        public static IEnumerable<Vector2Grid> CalculateVisibility(Map map, int row, int col, int maxRange)
        {
            var result = new List<Vector2Grid>();
            var dims = map.GetDimensions();
            var table = new int[dims.Row, dims.Col];
            if (!map.IsInBound(row, col)) return result;
            for (var i = 0; i < 8; i++)
            {
                BuildOctant(row, col, maxRange, i, map, table);
            }

            for (var i = Math.Max(0, row - maxRange); i < Math.Min(dims.Row, row + maxRange); i++)
            {
                for (var j = Math.Max(0, col - maxRange); j < Math.Min(dims.Col, col + maxRange); j++)
                {
                    if(table[i, j] == 1) result.Add(new Vector2Grid(i, j));
                }
            }

            return result;
        }
        
        private static void BuildOctant(int startingRow, int startingCol, int maxRange, 
                                        int i, Map map, int[,] table)
        {
            var shadows = new FOVShadowSet();
            var fullShadow = false;
            var rangeSqr = maxRange * maxRange;
            for (var row = 0; row <= maxRange && !fullShadow; row++)
            {
                var actualRow = startingRow + row * Oct[i, 0];
                var actualCol = startingCol + row * Oct[i, 2];
                if (!map.IsInBound(actualRow, actualCol)) break;
                for (var col = 0; col <= row; col++)
                {
                    if (row * row + col * col >= rangeSqr) continue;
                    actualRow = startingRow + row * Oct[i, 0] + col * Oct[i, 1];
                    actualCol = startingCol + row * Oct[i, 2] + col * Oct[i, 3];
                    if (!map.IsInBound(actualRow, actualCol)) break;
                    
                    var outRes = .0;
                    var inShadow = shadows.IsInShadow(row, col, InShadowRatio, out outRes);
                    var visible = table[actualRow, actualCol] != -1 && !inShadow;

                    if (outRes < 1 &&
                        (table[actualRow, actualCol] == -1 
                         || map.IsTileBlockingVisibility(actualRow, actualCol)))
                    {
                        shadows.Add(row, col);
                        fullShadow = shadows.IsFullShadow();
                    }
                    
                    table[actualRow, actualCol] = visible ? 1 : -1;
                }
            }
        }
    }
}