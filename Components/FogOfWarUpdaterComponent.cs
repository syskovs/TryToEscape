using TryToEscape.World;
using TryToEscape.Core;
using Microsoft.Xna.Framework;

namespace TryToEscape.Components;

public class FogOfWarUpdaterComponent : Component
{
    private FogOfWar _fog;
    private int _tileSize;
    private int _radius;

    public FogOfWarUpdaterComponent(FogOfWar fog, int tileSize, int radius)
    {
        _fog = fog;
        _tileSize = tileSize;
        _radius = radius;
    }

    public override void Update(GameTime gameTime)
    {
        var x = (int)((Owner.Position.X + _tileSize / 2f) / _tileSize);
        var y = (int)((Owner.Position.Y + _tileSize / 2f) / _tileSize);

        _fog.UpdateFrom(x, y, _radius);
    }
}