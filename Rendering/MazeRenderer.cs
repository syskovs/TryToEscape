using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TryToEscape.World;

namespace TryToEscape.Rendering;

public class MazeRenderer
{
    private Maze _maze;
    private Texture2D _floor;
    private Texture2D _wall;
    private Texture2D _exit;
    private int _tileSize;

    public MazeRenderer(Maze maze, Texture2D floor, Texture2D wall, Texture2D exit, int tileSize)
    {
        _maze = maze;
        _floor = floor;
        _wall = wall;
        _tileSize = tileSize;
        _exit = exit;
    }

    public void Draw(SpriteBatch spriteBatch, Rectangle visibleArea)
    {
        var bounds = TileBounds.FromVisibleArea(visibleArea, _tileSize, _maze.Width, _maze.Height);

        for (var x = bounds.Left; x < bounds.Right; x++)
            for (var y = bounds.Top; y < bounds.Bottom; y++)
            {
                var tile = _maze.GetTile(x ,y);

                if (tile.Type == Tile.TileType.Floor)
                    spriteBatch.Draw(_floor, new Rectangle(x * _tileSize, y * _tileSize, _tileSize, _tileSize), Color.White);
                else if (tile.Type == Tile.TileType.Wall)
                    spriteBatch.Draw(_wall, new Rectangle(x * _tileSize, y * _tileSize, _tileSize, _tileSize), Color.White);
                else if (tile.Type == Tile.TileType.Exit)
                    spriteBatch.Draw(_exit, new Rectangle(x * _tileSize, y * _tileSize, _tileSize, _tileSize), Color.White);
            }
    }
}