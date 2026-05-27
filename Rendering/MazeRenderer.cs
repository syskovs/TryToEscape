using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TryToEscape.World;

namespace TryToEscape.Rendering;

public class MazeRenderer
{
    private Maze _maze;
    private Texture2D _floor;
    private Texture2D _wall;
    private int _tileSize;

    public MazeRenderer(Maze maze, Texture2D floor, Texture2D wall, int tileSize)
    {
        _maze = maze;
        _floor = floor;
        _wall = wall;
        _tileSize = tileSize;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        for (var x = 0; x < _maze.Width; x++)
            for (var y = 0; y < _maze.Height; y++)
            {
                var tile = _maze.GetTile(x ,y);
                var vector2 = new Vector2(x * _tileSize, y * _tileSize);
                var color = Color.White;

                if (tile.Type == Tile.TileType.Floor)
                    spriteBatch.Draw(_floor, new Rectangle(x * _tileSize, y * _tileSize, _tileSize, _tileSize), Color.White);
                else if (tile.Type == Tile.TileType.Wall)
                    spriteBatch.Draw(_wall, new Rectangle(x * _tileSize, y * _tileSize, _tileSize, _tileSize), Color.White);
            }
    }
}