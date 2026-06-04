using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TryToEscape.Components;
using TryToEscape.Core;

namespace TryToEscape.Scenes;

public class VictoryScene : Scene
{
    private ContentManager _content;
    private GraphicsDevice _graphics;
    private SceneManager _sceneManager;
    private Texture2D _background;

    public VictoryScene(ContentManager content, GraphicsDevice graphics, SceneManager sceneManager)
    {
        _content = content;
        _graphics = graphics;
        _sceneManager = sceneManager;

        _background = content.Load<Texture2D>("assets/scenes/backgrounds/victory");
        var buttonSprite = content.Load<Texture2D>("assets/scenes/buttons/menu");
        
        var toMenuButton = new Entity();
        toMenuButton.AddComponent(new SpriteComponent(buttonSprite));
        toMenuButton.Position = new Vector2(
            (_graphics.Viewport.Width  - buttonSprite.Width)  / 2,
            (_graphics.Viewport.Height - buttonSprite.Height) / 2 + 200);
        toMenuButton.AddComponent(new ButtonComponent(
            buttonSprite.Width, 
            buttonSprite.Height,
            () => _sceneManager.Replace(new MenuScene(_content, _graphics, _sceneManager))));
        AddEntity(toMenuButton);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Begin();
        spriteBatch.Draw(_background, 
            new Rectangle(0, 0, _graphics.Viewport.Width, _graphics.Viewport.Height), 
            Color.White);
        base.Draw(spriteBatch);
        spriteBatch.End();
    }
}