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
    private int _keyToExit;

    public ExitDetectorComponent(Maze maze, int keyToExit, int tileSize, Action onExit)
    {
        _maze = maze;
        _tileSize = tileSize;
        _onExit = onExit;
        _keyToExit = keyToExit;
    }

    public override void Update(GameTime gameTime)
    {
        var inv = Owner.GetComponent<InventoryComponent>();

        if (inv == null || !inv.HasKey) return;

        var position = Owner.Position.ToTileCentered(_tileSize);
        var tile =_maze.GetTile(position.X, position.Y);

        if (tile.Type == Tile.TileType.Exit && inv.KeyCount >= _keyToExit)
            _onExit();
    }
}