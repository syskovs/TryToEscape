using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TryToEscape.Core;

namespace TryToEscape.Components;

public class SpriteComponent : Component
{
    public Texture2D Texture { get; private set; }

    public SpriteComponent(Texture2D texture)
    {
        Texture = texture;
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(Texture, Owner.Position, Color.White);
    }
}