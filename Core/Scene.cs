using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TryToEscape.Core;

public class Scene
{
    private readonly List<Entity> _entities = new();
    private readonly List<Entity> _toRemove = new();

    public void AddEntity(Entity entity)
    {
        _entities.Add(entity);
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
        
        foreach (var e in _toRemove)
            _entities.Remove(e);
        _toRemove.Clear();
    }

    public virtual void Draw(SpriteBatch spriteBatch)
    {
        foreach (var e in _entities)
            e.Draw(spriteBatch);
    }

    public void RemoveEntity(Entity entity) 
    {
        _toRemove.Add(entity);
    }
}