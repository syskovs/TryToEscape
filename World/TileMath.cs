using Microsoft.Xna.Framework;

namespace TryToEscape.World;

public static class TileMath
{
    public static Point ToTile(this Vector2 pixel, int tileSize)
        => new Point((int)(pixel.X / tileSize), (int)(pixel.Y / tileSize));

    public static Vector2 ToPixel(this Point tile, int tileSize)
        => new Vector2(tile.X * tileSize, tile.Y * tileSize);

    public static Point ToTileCentered(this Vector2 topLeft, int tileSize)
    => (topLeft + new Vector2(tileSize / 2f)).ToTile(tileSize);
}