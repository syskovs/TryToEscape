using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TryToEscape.Rendering;

namespace TryToEscape.Core;

public class Scene
{
    readonly List<Entity> _entities = new();

    public void AddEntity(Entity entity)
    {
        _entities.Add(entity);
    }

    public virtual Matrix GetCameraMatrix()
    {
        return Matrix.Identity;
    }

    public virtual void PreUpdate(GameTime gameTime)
    {
        foreach (var e in _entities)
            e.PreUpdate(gameTime);
    }
    
    public virtual void Update(GameTime gameTime)
    {
        foreach (var e in _entities)
            e.Update(gameTime);
    }

    public virtual void Draw(SpriteBatch spriteBatch)
    {
        foreach (var e in _entities)
            e.Draw(spriteBatch);
    }
}