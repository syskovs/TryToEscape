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

    public ColliderComponent(Maze maze, int size, int tileSize)
    {
        _maze = maze;
        _size = size;
        _tileSize = tileSize;
    }

    public override void Update(GameTime gameTime)
    {
        var x = (int)(Owner.Position.X / _tileSize);
        var y = (int)(Owner.Position.Y / _tileSize);

        var tile = _maze.GetTile(x, y);

        if (tile.Type == Tile.TileType.Wall)
        {
            x -= _size;
            y -= _size;
        }

        base.Update(gameTime);
    }
}