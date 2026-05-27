using System;
using Microsoft.Xna.Framework;

namespace TryToEscape.World;

public class MazeGenerator
{
    private readonly Random _rng = new Random();

    public Maze Generate(int width, int height)
    {
        var maze = new Maze(width, height);

        return maze;
    }

    private void Split(BSPNode node, int minSize)
    {
        if (!CanSplit(node, minSize))
            return;
        
        var splitVertical = _rng.Next(0, 2) == 0;
        var splitRatio = _rng.Next(40, 60) / 100f;

        if (splitVertical)
            SplitVertical(node, splitRatio);
        else
            SplitHorizontal(node, splitRatio);

        Split(node.Left, minSize);
        Split(node.Right, minSize);
    }

    private void CreateRooms(BSPNode node, Maze maze)
    {
        
    }

    private void ConnectRooms(BSPNode node, Maze maze)
    {
        
    }

    private bool CanSplit(BSPNode node, int minSize)
    {
        return node.Area.Width >= minSize * 2 && node.Area.Height >= minSize * 2 && node.Left == null;
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