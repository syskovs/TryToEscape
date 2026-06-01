using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TryToEscape.Core;

namespace TryToEscape.Components;

public class SpriteComponent : Component
{
    private Texture2D _texture;
    public float Scale { get; set; } = 1f;

    public SpriteComponent(Texture2D texture)
    {
        _texture = texture;
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        var w = _texture.Width  * Scale;
        var h = _texture.Height * Scale;
        var centerX = Owner.Position.X + _texture.Width  / 2f;
        var centerY = Owner.Position.Y + _texture.Height / 2f;

        var destRect = new Rectangle(
            (int)(centerX - w / 2),
            (int)(centerY - h / 2),
            (int)w, (int)h);

        spriteBatch.Draw(_texture, destRect, Color.White);
    }
}