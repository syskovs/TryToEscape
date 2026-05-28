using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace TryToEscape.World;

public static class VisibilityCalculator
{
    public static IEnumerable<(int x, int y)> Compute(Maze maze, int originX, int originY, int radius)
    {
        for (var x = originX - radius; x <= originX + radius; x++)
        {
            for (var y = originY - radius; y <= originY + radius; y++)
            {
                if (x < 0 || x >= maze.Width || y < 0 || y >= maze.Height)
                    continue;

                yield return (x, y);
            }
        }  
    }
}