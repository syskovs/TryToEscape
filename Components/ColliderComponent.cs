using TryToEscape.World;
using TryToEscape.Core;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace TryToEscape.Components;

public class ColliderComponent : Component
{
    private Maze _maze;
    private int _size;
    private int _tileSize;
    private Vector2 _previousPosition;

    public ColliderComponent(Maze maze, int size, int tileSize)
    {
        _maze = maze;
        _size = size;
        _tileSize = tileSize;
    }

    public override void PreUpdate(GameTime gameTime)
    {
        _previousPosition = Owner.Position;
    }

    public override void Update(GameTime gameTime)
    {
        var x = Owner.Position.X;
        var y = Owner.Position.Y;

        if (IsWallAt(x, y) || IsWallAt(x + _size - 1, y) ||
            IsWallAt(x, y + _size - 1) || IsWallAt(x + _size - 1, y + _size - 1))
            Owner.Position = _previousPosition;
    }

    private bool IsWallAt(float px, float py)
    {
        var x = (int)(px / _tileSize);
        var y = (int)(py / _tileSize);
        var tile = _maze.GetTile(x, y);

        return tile.Type == Tile.TileType.Wall;
    }
}