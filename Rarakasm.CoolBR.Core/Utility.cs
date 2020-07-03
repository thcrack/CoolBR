using System;
using Rarakasm.CoolBR.Core.System.FieldOfView;
using Rarakasm.CoolBR.Core.World;

namespace Rarakasm.CoolBR.Core
{
    public class Utility
    {
        
        public void TestFOV(Map map, int row, int col)
        {
            var visibles = FOVCalculator.CalculateVisibility(map, row, col, 5);
            var mapString = map.GetFormattedTileStrings();
            foreach (var visible in visibles)
            {
                mapString[visible.Row][visible.Col] = '2';
            }

            foreach (var ca in mapString)
            {
                foreach (var c in ca)
                {
                    Console.Write(c);
                }
                Console.WriteLine();
            }

            Console.Read();
        }
    }
}