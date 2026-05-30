using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;

namespace TryToEscape.Core;

public class SceneManager
{
    private Stack<Scene> _scenes = new();

    public SceneManager() {}

    public void PreUpdate(GameTime gameTime)
    {
        if (_scenes.Count == 0) 
            return;
        
        _scenes.Peek().PreUpdate(gameTime);
    }

    public void Update(GameTime gameTime)
    {
        if (_scenes.Count == 0) 
            return;
        
        _scenes.Peek().Update(gameTime);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        foreach (var s in _scenes.Reverse())
            s.Draw(spriteBatch);
    }

    public void Push(Scene scene) 
    {
        _scenes.Push(scene);
    }

    public void Pop()
    {
        _scenes.Pop();
    }

    public void Replace(Scene scene) 
    {
        _scenes.Clear();
        _scenes.Push(scene);
    }
}