namespace TryToEscape.World;

public class Maze
{
    public int Width { get; }
    public int Height { get; }
    private readonly Tile[,] _tiles;

    public Maze(int width, int height)
    {
        _tiles = new Tile[width, height];

        Width = width;
        Height = height;
    }

    public Tile GetTile(int x, int y)
    {
        return _tiles[x, y];
    }
}