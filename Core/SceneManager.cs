using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TryToEscape.Core;

public class SceneManager
{
    Scene _currentScene;

    public SceneManager(Scene scene)
    {
        _currentScene = scene;
    }

    public void ChangeScene(Scene scene)
    {
        _currentScene = scene;
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