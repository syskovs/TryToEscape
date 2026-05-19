using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TryToEscape.Core;

public abstract class Component
{
    public Entity Owner { get; internal set; }

    public virtual void Update(GameTime gameTime) {}

    public virtual void Draw(SpriteBatch spriteBatch) {}
}