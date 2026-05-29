using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TryToEscape.Core;

public class SceneManager
{
    Scene _currentScene;

    public SceneManager() {}

    public Matrix GetCameraMatrix()
    {
        return _currentScene.GetCameraMatrix();
    }

    public void ChangeScene(Scene scene)
    {
        _currentScene = scene;
    }

    public void PreUpdate(GameTime gameTime)
    {
        _currentScene.PreUpdate(gameTime);
    }

    public void Update(GameTime gameTime)
    {
        _currentScene.Update(gameTime);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        _currentScene.Draw(spriteBatch);
    }

}