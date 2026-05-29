using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TryToEscape.World;

namespace TryToEscape.Rendering;

public class FogOfWarRenderer
{
    private FogOfWar _fog;
    private Texture2D _pixel;
    private int _tileSize;


    public FogOfWarRenderer(FogOfWar fog, Texture2D pixel, int tileSize)
    {
        _fog = fog;
        _pixel = pixel;
        _tileSize = tileSize;
    }

    public void Draw(SpriteBatch spriteBatch, Rectangle visibleArea)
    {
        var bounds = TileBounds.FromVisibleArea(visibleArea, _tileSize, _fog.Width, _fog.Height);

        for (var x = bounds.Left; x < bounds.Right; x++)
            for (var y = bounds.Top; y < bounds.Bottom; y++)
            {
                var state = _fog.GetState(x, y);
                
                if (state == FogOfWar.State.Visible) continue;

                var alpha = state switch {
                    FogOfWar.State.Hidden => 1f,
                    FogOfWar.State.Explored => 0.6f,
                    _ => 0f
                };

                spriteBatch.Draw(_pixel, new Rectangle(x * _tileSize, y * _tileSize, _tileSize, _tileSize), Color.Black * alpha);
            }
    }
}