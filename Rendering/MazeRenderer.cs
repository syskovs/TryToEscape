using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TryToEscape.World;

namespace TryToEscape.Rendering;

public class MazeRenderer
{
    private Maze _maze;
    private Texture2D _atlas;
    private Rectangle _exitRect;
    private Dictionary<int, Rectangle[]> _wallRects;
    private Dictionary<int, Rectangle[]> _floorRects;
    private Rectangle _defaultFloorRect;
    private Rectangle _defaultWallRect;
    private int _tileSize;

    public MazeRenderer(
        Maze maze,
        Texture2D atlas,
        Dictionary<int, Rectangle[]> floorRects,
        Rectangle defaultFloorRect,
        Dictionary<int, Rectangle[]> wallRects,
        Rectangle defaultWallRect,
        Rectangle exitRect,
        int tileSize)
    {
        _maze = maze;
        _atlas = atlas;
        _wallRects = wallRects;
        _floorRects = floorRects;
        _defaultWallRect = defaultWallRect;
        _defaultFloorRect = defaultFloorRect;
        _exitRect = exitRect;
        _tileSize = tileSize;
    }

    public void Draw(SpriteBatch spriteBatch, Rectangle visibleArea)
    {
        var bounds = TileBounds.FromVisibleArea(visibleArea, _tileSize, _maze.Width, _maze.Height);

        for (var x = bounds.Left; x < bounds.Right; x++)
            for (var y = bounds.Top; y < bounds.Bottom; y++)
            {
                var tile = _maze.GetTile(x, y);
                var destRect = new Rectangle(x * _tileSize, y * _tileSize, _tileSize, _tileSize);

                if (tile.Type == Tile.TileType.Floor)
                {
                    var mask = ComputeFloorMask(x, y);
                    var variants = _floorRects.GetValueOrDefault(mask);
                    var rect = (variants == null || variants.Length == 0)
                        ? _defaultFloorRect
                        : variants[HashPosition(x, y) % variants.Length];

                    spriteBatch.Draw(_atlas, destRect, rect, Color.White);
                }
                else if (tile.Type == Tile.TileType.Wall)
                {
                    var mask = ComputeWallMask(x, y);
                    var variants = _wallRects.GetValueOrDefault(mask);
                    var rect = (variants == null || variants.Length == 0)
                        ? _defaultWallRect
                        : variants[HashPosition(x, y) % variants.Length];

                    spriteBatch.Draw(_atlas, destRect, rect, Color.White);
                }
                else if (tile.Type == Tile.TileType.Exit)
                {
                    spriteBatch.Draw(_atlas, destRect, _exitRect, Color.White);
                }
            }
    }

    private int ComputeFloorMask(int x, int y)
    {
        int mask = 0;
        if (IsWallInBounds(x,   y-1)) mask |= 1;
        if (IsWallInBounds(x+1, y))   mask |= 2;
        if (IsWallInBounds(x,   y+1)) mask |= 4;
        if (IsWallInBounds(x-1, y))   mask |= 8;
        return mask;
    }

    private bool IsWallInBounds(int x, int y)
    {
        if (x < 0 || x >= _maze.Width || y < 0 || y >= _maze.Height)
            return false;
        return _maze.GetTile(x, y).Type == Tile.TileType.Wall;
    }
    private int ComputeWallMask(int x, int y)
    {
        bool floorN = IsFloorInBounds(x,   y-1);
        bool floorE = IsFloorInBounds(x+1, y);
        bool floorS = IsFloorInBounds(x,   y+1);
        bool floorW = IsFloorInBounds(x-1, y);

        int cardinal = 0;
        if (floorN) cardinal |= 1;
        if (floorE) cardinal |= 2;
        if (floorS) cardinal |= 4;
        if (floorW) cardinal |= 8;
        if (cardinal != 0) return cardinal;

        if (IsFloorInBounds(x-1, y+1)) return 16;
        if (IsFloorInBounds(x+1, y+1)) return 17;
        if (IsFloorInBounds(x-1, y-1)) return 18;
        if (IsFloorInBounds(x+1, y-1)) return 19;

        return 0;
    }

    private bool IsFloorInBounds(int x, int y)
    {
        if (x < 0 || x >= _maze.Width || y < 0 || y >= _maze.Height)
            return false;
        return _maze.GetTile(x, y).Type != Tile.TileType.Wall;
    }

    private int HashPosition(int x, int y)
    {
        return Math.Abs(x * 73 + y * 17);
    }
}