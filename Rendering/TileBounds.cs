using System;
using Microsoft.Xna.Framework;

namespace TryToEscape.Rendering;

public static class TileBounds
{
    public static Rectangle FromVisibleArea(Rectangle area, int tileSize, int maxWidth, int maxHeight) 
    {
        var tileXStart = Math.Max(0, area.Left / tileSize);
        var tileXEnd = Math.Min(maxWidth, area.Right / tileSize + 1);
        var tileYStart = Math.Max(0, area.Top / tileSize);
        var tileYEnd   = Math.Min(maxHeight, area.Bottom / tileSize + 1);

        return new Rectangle(
            tileXStart,
            tileYStart,
            tileXEnd - tileXStart,
            tileYEnd - tileYStart
        );
    }
}