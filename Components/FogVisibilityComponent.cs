using Microsoft.Xna.Framework;
using TryToEscape.Core;
using TryToEscape.World;

namespace TryToEscape.Components;

public class FogVisibilityComponent : Component
{
    private FogOfWar _fog;
    private int _tileSize;

    public FogVisibilityComponent(FogOfWar fog, int tileSize)
    {
        _fog = fog;
        _tileSize = tileSize;
    }

    public override void Update(GameTime gameTime)
    {
        var tile = Owner.Position.ToTileCentered(_tileSize);
        var state = _fog.GetState(tile.X, tile.Y);
        Owner.Visible = state == FogOfWar.State.Visible;
    }
}