using Microsoft.Xna.Framework;

namespace TryToEscape.World;

public class BSPNode
{
    public Rectangle Area;
    public Rectangle? Room;
    public BSPNode Left;
    public BSPNode Right;

    public BSPNode(Rectangle area)
    {
        Area = area;
    }
}