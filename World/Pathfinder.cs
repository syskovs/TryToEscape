using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using static TryToEscape.World.Tile;

namespace TryToEscape.World;

public static class Pathfinder
{
    private static readonly Point[] CardinalDirections =
    {
        new Point(0, -1),
        new Point(0, 1),
        new Point(-1, 0),
        new Point(1, 0)
    };

    public static List<Point> FindPath(Maze maze, Point start, Point end)
    {
        var queue = new Queue<Point>();
        var cameFrom = new Dictionary<Point, Point>();

        queue.Enqueue(start);
        cameFrom[start] = start;

        while (queue.Count > 0)
        {
            Point current = queue.Dequeue();

            if (current == end)
                return ReconstructPath(cameFrom, start, end);

            var directions = GetSortedDirections(current, end);

            foreach (Point direction in directions)
            {
                Point neighbor = new Point(
                    current.X + direction.X,
                    current.Y + direction.Y);

                if (!InBounds(maze, neighbor)) continue;
                if (maze.GetTile(neighbor.X, neighbor.Y).Type == TileType.Wall) continue;
                if (cameFrom.ContainsKey(neighbor)) continue;

                cameFrom[neighbor] = current;
                queue.Enqueue(neighbor);
            }
        }

        return new List<Point>();
    }

    private static Point[] GetSortedDirections(Point current, Point end)
    {
        var directions = (Point[])CardinalDirections.Clone();
        var goalDx = end.X - current.X;
        var goalDy = end.Y - current.Y;

        Array.Sort(directions, (a, b) =>
        {
            int dotA = a.X * goalDx + a.Y * goalDy;
            int dotB = b.X * goalDx + b.Y * goalDy;
            return dotB.CompareTo(dotA);
        });

        return directions;
    }

    private static List<Point> ReconstructPath(
        Dictionary<Point, Point> cameFrom,
        Point start,
        Point end)
    {
        var path = new List<Point>();
        Point current = end;

        while (current != start)
        {
            path.Add(current);
            current = cameFrom[current];
        }

        path.Add(start);
        path.Reverse();

        return path;
    }

    private static bool InBounds(Maze maze, Point point)
    {
        return point.X >= 0 &&
            point.Y >= 0 &&
            point.X < maze.Width &&
            point.Y < maze.Height;
    }
}