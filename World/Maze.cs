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

        for (var x = 0; x < width; x++)
            for (var y = 0; y < height; y++)
                SetTile(x, y, new Tile(Tile.TileType.Wall, x, y));
    }

    public Tile GetTile(int x, int y) => _tiles[x, y];

    public void SetTile(int x, int y, Tile tile)
    {
        _tiles[x, y] = tile;
    }
}