using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TryToEscape.Core;

public class Entity
{
    readonly List<Component> _components = new();
    public Vector2 Position;
    public bool Visible { get; set; } = true;

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
    public void PreUpdate(GameTime gameTime)
    {
        foreach (var c in _components)
            c.PreUpdate(gameTime);
    }

    public void Update(GameTime gameTime)
    {
        foreach (var c in _components)
            c.Update(gameTime);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        if (!Visible) return;
        
        foreach (var c in _components)
            c.Draw(spriteBatch);
    }
}