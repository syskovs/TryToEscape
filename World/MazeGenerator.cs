using System;
using Microsoft.Xna.Framework;

namespace TryToEscape.World;

public class MazeGenerator
{
    private readonly Random _rng = new Random();
    private int _padding;
    private int _minSize;
    private int _minRoomSize;

    public Maze Generate(int width, int height, int minSize, int minRoomSize, int padding)
    {
        _minSize = minSize;
        _padding = padding;
        _minRoomSize = minRoomSize;

        var maze = new Maze(width, height);
        var node = new BSPNode(new Rectangle(0, 0, maze.Width, maze.Height));

        Split(node);
        CreateRooms(node, maze);
        ConnectRooms(node, maze);

        return maze;
    }

    private void Split(BSPNode node)
    {
        if (!CanSplit(node))
            return;
        
        var splitVertical = _rng.Next(0, 2) == 0;
        var splitRatio = _rng.Next(40, 60) / 100f;

        if (splitVertical)
            SplitVertical(node, splitRatio);
        else
            SplitHorizontal(node, splitRatio);

        Split(node.Left);
        Split(node.Right);
    }

    private void CreateRooms(BSPNode node, Maze maze)
    {
        if (node.Left != null || node.Right != null)
        {
            CreateRooms(node.Left, maze);
            CreateRooms(node.Right, maze);
            return;
        }

        var maxWidth = node.Area.Width - _padding * 2;
        var maxHeight = node.Area.Height - _padding * 2;

        if (maxWidth < _minRoomSize || maxHeight < _minRoomSize) return;

        var roomWidth = _rng.Next(_minRoomSize, maxWidth);
        var roomHeight = _rng.Next(_minRoomSize, maxHeight);

        var roomX = _rng.Next(node.Area.X + _padding, node.Area.X + node.Area.Width - roomWidth - _padding);
        var roomY = _rng.Next(node.Area.Y + _padding, node.Area.Y + node.Area.Height - roomHeight - _padding);

        node.Room = new Rectangle(roomX, roomY, roomWidth, roomHeight);

        for (var x = roomX; x < roomX + roomWidth; x++)
            for (var y = roomY; y < roomY + roomHeight; y++)
                maze.SetTile(x, y, new Tile(Tile.TileType.Floor, x, y));

    }

    private void ConnectRooms(BSPNode node, Maze maze)
    {
        
    }

    private bool CanSplit(BSPNode node)
    {
        return node.Area.Width >= _minSize * 2 && node.Area.Height >= _minSize * 2 && node.Left == null;
    }

    private void SplitVertical(BSPNode node, float splitRatio)
    {
        var splitWidth = (int)(node.Area.Width * splitRatio);
        var splitX = node.Area.X + splitWidth;
        node.Left  = new BSPNode(new Rectangle(node.Area.X, node.Area.Y, splitWidth, node.Area.Height));
        node.Right = new BSPNode(new Rectangle(splitX, node.Area.Y, node.Area.Width - splitWidth, node.Area.Height));
    }

    private void SplitHorizontal(BSPNode node, float splitRatio)
    {
        var splitHeight = (int)(node.Area.Height * splitRatio);
        var splitY = node.Area.Y + splitHeight;
        node.Left  = new BSPNode(new Rectangle(node.Area.X, node.Area.Y, node.Area.Width, splitHeight));
        node.Right = new BSPNode(new Rectangle(node.Area.X, splitY, node.Area.Width, node.Area.Height - splitHeight));
    }
}