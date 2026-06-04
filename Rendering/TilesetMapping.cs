using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace TryToEscape.Rendering;

public static class TilesetMapping
{
    private static Rectangle Rect(int col, int row) => new(col * 16, row * 16, 16, 16);

    public static Rectangle DefaultFloor => Rect(9, 0);
    public static Rectangle DefaultWall => Rect(0, 0);
    public static Rectangle Exit => Rect(2, 8);

    public static Dictionary<int, Rectangle[]> BuildWallRects()
    {
        return new Dictionary<int, Rectangle[]>
        {
            { 0,  new[] { Rect(8, 7) } },

            { 1,  new[] {
                Rect(1, 4),
                Rect(2, 4),
                Rect(3, 4),
                Rect(4, 4)
            }},

            { 2,  new[] {
                Rect(0, 1),
                Rect(0, 2),
                Rect(0, 3)
            }},

            { 3,  new[] { Rect(3, 5) } },

            { 4,  new[] {
                Rect(1, 0),
                Rect(2, 0),
                Rect(3, 0),
                Rect(4, 0)
            }},

            { 5,  new[] {
                Rect(1, 0),
                Rect(2, 0),
                Rect(3, 0),
                Rect(4, 0)
            }},

            { 6,  new[] {
                Rect(1, 0),
                Rect(2, 0),
                Rect(3, 0),
                Rect(4, 0)
            }},

            { 7,  new[] {
                Rect(1, 0),
                Rect(2, 0),
                Rect(3, 0),
                Rect(4, 0)
            }},

            { 8,  new[] {
                Rect(5, 1),
                Rect(5, 2),
                Rect(5, 3)
            }},

            { 9,  new[] { Rect(0, 5) } },

            { 10, new[] { Rect(4, 6) } },

            { 11, new[] { Rect(4, 6) } },

            { 12, new[] {
                Rect(1, 0),
                Rect(2, 0),
                Rect(3, 0),
                Rect(4, 0)
            }},

            { 13, new[] {
                Rect(1, 0),
                Rect(2, 0),
                Rect(3, 0),
                Rect(4, 0)
            }},

            { 14, new[] { Rect(1, 0) } },

            { 15, new[] { Rect(1, 0) } },

            { 16, new[] { Rect(5, 0) } },

            { 17, new[] { Rect(0, 0) } },

            { 18, new[] { Rect(5, 4) } },

            { 19, new[] { Rect(0, 4) } },
        };
    }

    

    public static Dictionary<int, Rectangle[]> BuildFloorRects()
    {
        return new Dictionary<int, Rectangle[]>
        {
            { 0, new[] {
                Rect(6, 0),
                Rect(7, 0),
                Rect(8, 0),
                Rect(9, 0),
                Rect(6, 1),
                Rect(7, 1),
                Rect(8, 1),
                Rect(9, 1),
                Rect(6, 2),
                Rect(7, 2),
                Rect(8, 2),
                Rect(9, 2)
            }},

            { 1, new[] {
                Rect(2, 1),
                Rect(3, 1)
            }},

            { 2, new[] { Rect(4, 2) } },

            { 3, new[] { Rect(4, 1) } },

            { 4, new[] {
                Rect(2, 3),
                Rect(3, 3)
            }},

            { 6, new[] { Rect(4, 3) } },

            { 8, new[] { Rect(1, 2) } },

            { 9, new[] { Rect(1, 1) } },

            { 12, new[] { Rect(1, 3) } },
        };
    }
}