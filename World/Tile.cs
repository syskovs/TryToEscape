namespace TryToEscape.World;

public class Tile
{
    public int X { get; }
    public int Y { get; }
    public TileType Type;

    public enum TileType
    {
        Floor,
        Door,
        Wall,
        Exit
    }

    public Tile(TileType type, int x, int y)
    {
        Type = type;
        X = x;
        Y = y;
    }


}