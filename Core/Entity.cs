using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TryToEscape.Core;

public class Entity
{
    readonly List<Component> _components = new();
    public Vector2 Position;

    public void AddComponent(Component component)
    {
        component.Owner = this;
        _components.Add(component);
    }

    public T GetComponent<T>() where T : Component
    {
        foreach (var c in _components)
            if (c is T result)
                return result;

        return default;
    }

    public void Update(GameTime gameTime)
    {
        foreach (var c in _components)
            c.Update(gameTime);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        foreach (var c in _components)
            c.Draw(spriteBatch);
    }
}