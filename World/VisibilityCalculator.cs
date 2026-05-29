using System;
using System.Collections.Generic;

namespace TryToEscape.World;

public static class VisibilityCalculator
{
    public static IEnumerable<(int x, int y)> Compute(Maze maze, int originX, int originY, int radius)
    {
        for (var x = originX - radius; x <= originX + radius; x++)
        {
            for (var y = originY - radius; y <= originY + radius; y++)
            {
                var dx = x - originX;
                var dy = y - originY;

                if (dx * dx + dy * dy > radius * radius) 
                    continue;

                if (x < 0 || x >= maze.Width || y < 0 || y >= maze.Height)
                    continue;
                
                if (HasLineOfSight(maze, originX, originY, x, y))
                    yield return (x, y);
            }
        }
    }

    private static bool HasLineOfSight(Maze maze, int fromX, int fromY, int toX, int toY)
    {
        var dx = Math.Abs(fromX - toX);
        var dy = Math.Abs(fromY - toY);
        var sx = (fromX < toX) ? 1 : -1;
        var sy = (fromY < toY) ? 1 : -1;
        var err = dx - dy;
        var x = fromX;
        var y = fromY;

        while (true)
        {
            var e2 = 2 * err;

            if (e2 > -dy)
            {
                err -= dy;
                x += sx;
            }

            if (e2 < dx)
            {
                err += dx;
                y += sy;
            }

            if ((x, y) == (toX, toY))
                return true;
            
            var tile = maze.GetTile(x, y);
            if (tile.Type == Tile.TileType.Wall)
                return false;
        } 
    }
}