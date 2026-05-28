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

    public void Draw(SpriteBatch spriteBatch)
    {
        for (var x = 0; x < _fog.Width; x++)
            for (var y = 0; y < _fog.Height; y++)
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