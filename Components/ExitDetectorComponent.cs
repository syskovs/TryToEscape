using System;
using Microsoft.Xna.Framework;
using TryToEscape.Core;
using TryToEscape.World;

namespace TryToEscape.Components;

public class ExitDetectorComponent : Component
{
    private Maze _maze;
    private int _tileSize;
    private Action _onExit;

    public ExitDetectorComponent(Maze maze, int tileSize, Action onExit)
    {
        _maze = maze;
        _tileSize = tileSize;
        _onExit = onExit;
    }

    public override void Update(GameTime gameTime)
    {
        var inv = Owner.GetComponent<InventoryComponent>();

        if (inv == null || !inv.HasKey) return;

        var x = (int)((Owner.Position.X + _tileSize / 2f) / _tileSize);
        var y = (int)((Owner.Position.Y + _tileSize / 2f) / _tileSize);

        var tile =_maze.GetTile(x, y);

        if (tile.Type == Tile.TileType.Exit)
            _onExit();
    }
}