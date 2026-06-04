using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System.Linq;

namespace TryToEscape.World;

public class MazeGenerator
{
    private readonly Random _rng = new Random();
    private int _padding;
    private int _minSize;
    private int _minRoomSize;
    private BSPNode _root;
    private List<BSPNode> _leaves;

    public Maze Generate(int width, int height, int minSize, int minRoomSize, int padding)
    {
        _leaves = new();
        _minSize = minSize;
        _padding = padding;
        _minRoomSize = minRoomSize;

        var maze = new Maze(width, height);
        var node = new BSPNode(new Rectangle(0, 0, maze.Width, maze.Height));
        _root = node;

        Split(node);
        CreateRooms(node, maze);
        ConnectRooms(node, maze);

        var endPos = GetEndPosition();
        maze.GetTile((int)endPos.X, (int)endPos.Y).Type = Tile.TileType.Exit;   

        return maze;
    }

    public Point GetRandomFloorPosition()
    {
        var choice = _rng.Next(_leaves.Count);

        var currentLeaf = _leaves[choice];
        var startLeaf = GetLeftmostLeaf(_root);
        var endLeaf = GetExitLeaf(_root);

        if (currentLeaf == startLeaf || currentLeaf == endLeaf)
            return GetRandomFloorPosition();

        var room = currentLeaf.Room.Value;
        
        return new Point(room.Center.X, room.Center.Y);
    }

    public IReadOnlyList<Point> GetRandomRooms(int count)
    {
        var startLeaf = GetLeftmostLeaf(_root);
        var endLeaf = GetExitLeaf(_root);
        var available = _leaves.Where(l => l != startLeaf && l != endLeaf).ToList();

        for (int i = available.Count - 1; i > 0; i--)
        {
            int j = _rng.Next(i + 1);
            (available[i], available[j]) = (available[j], available[i]);
        }

        return available.Take(count).Select(leaf => leaf.Room.Value.Center).ToList();
    }

    public Vector2 GetStartPosition()
    {
        var node = GetLeftmostLeaf(_root);
        var vector = new Vector2(node.Room.Value.Center.X, node.Room.Value.Center.Y);

        return vector;
    }

    public Vector2 GetEndPosition()
    {
        var node = GetExitLeaf(_root);
        var vector = new Vector2(node.Room.Value.Center.X, node.Room.Value.Center.Y);

        return vector;
    }

    private BSPNode GetLeftmostLeaf(BSPNode node)
    {
        if (IsLeaf(node))
            return node;
        
        return GetLeftmostLeaf(node.Left);
    }

    private BSPNode GetExitLeaf(BSPNode node)
    {
        if (IsLeaf(node))
            return node;

        return GetExitLeaf(node.Right);
    }

    private void Split(BSPNode node)
    {
        if (!CanSplit(node))
            return;
        
        
        var splitRatio = _rng.Next(40, 60) / 100f;
        bool splitVertical;

        if (node.Area.Width > node.Area.Height)
            splitVertical = true;
        else if (node.Area.Height > node.Area.Width)
            splitVertical = false;
        else
            splitVertical = _rng.Next(0, 2) == 0;

        if (splitVertical)
            SplitVertical(node, splitRatio);
        else
            SplitHorizontal(node, splitRatio);

        Split(node.Left);
        Split(node.Right);
    }

    private void CreateRooms(BSPNode node, Maze maze)
    {
        if (!IsLeaf(node))
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

        var roomOriginX = _rng.Next(node.Area.X + _padding, node.Area.X + node.Area.Width - roomWidth - _padding);
        var roomOriginY = _rng.Next(node.Area.Y + _padding, node.Area.Y + node.Area.Height - roomHeight - _padding);

        node.Room = new Rectangle(roomOriginX, roomOriginY, roomWidth, roomHeight);
        _leaves.Add(node);
        CarveRoom(node.Room.Value, maze);
    }

    private bool IsLeaf(BSPNode node)
    {
        return node.Left == null && node.Right == null;
    }

    private void ConnectRooms(BSPNode node, Maze maze)
    {
        if (IsLeaf(node)) return;

        ConnectRooms(node.Left, maze);
        ConnectRooms(node.Right, maze);

        var roomA = GetRoom(node.Left);
        var roomB = GetRoom(node.Right);

        if (roomA == null || roomB == null) return;

        var roomACenter = roomA.Value.Center;
        var roomBCenter = roomB.Value.Center;

        var corridorStartX = Math.Min(roomACenter.X, roomBCenter.X);
        var corridorEndX = Math.Max(roomACenter.X, roomBCenter.X);
        var corridorStartY = Math.Min(roomACenter.Y, roomBCenter.Y);
        var corridorEndY = Math.Max(roomACenter.Y, roomBCenter.Y);

        CarveHorizontalCorridor(roomACenter.Y, corridorStartX, corridorEndX, maze);
        CarveVerticalCorridor(roomBCenter.X, corridorStartY, corridorEndY, maze);
    }

    private Rectangle? GetRoom(BSPNode node)
    {
        if (node.Room != null)
            return node.Room;
        
        return GetRoom(node.Left) ?? GetRoom(node.Right);
    }

    private void CarveHorizontalCorridor(int y, int fromX, int toX, Maze maze)
    {
        for (var x = fromX; x <= toX; x++)
        {
            maze.SetTile(x, y, new Tile(Tile.TileType.Floor, x, y));
            maze.SetTile(x, y + 1, new Tile(Tile.TileType.Floor, x, y + 1));
        }
    }

    private void CarveVerticalCorridor(int x, int fromY, int toY, Maze maze)
    {
        for (var y = fromY; y <= toY; y++)
        {
            maze.SetTile(x, y, new Tile(Tile.TileType.Floor, x, y));
            maze.SetTile(x + 1, y, new Tile(Tile.TileType.Floor, x + 1, y));
        }
    }

    private void CarveRoom(Rectangle room, Maze maze)
    {
        for (var x = room.X; x < room.X + room.Width; x++)
            for (var y = room.Y; y < room.Y + room.Height; y++)
                maze.SetTile(x, y, new Tile(Tile.TileType.Floor, x, y));
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