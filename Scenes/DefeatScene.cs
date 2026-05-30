using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TryToEscape.Components;
using TryToEscape.Core;

namespace TryToEscape.Scenes;

public class DefeatScene : Scene
{
    private ContentManager _content;
    private GraphicsDevice _graphics;
    private SceneManager _sceneManager;

    public DefeatScene(ContentManager content, GraphicsDevice graphics, SceneManager sceneManager)
    {
        _content = content;
        _graphics = graphics;
        _sceneManager = sceneManager;

        var buttonSprite = content.Load<Texture2D>("assets/buttons/to_menu");
        
        var toMenuButton = new Entity();
        toMenuButton.AddComponent(new SpriteComponent(buttonSprite));
        toMenuButton.Position = new Vector2(
            (_graphics.Viewport.Width  - buttonSprite.Width)  / 2,
            (_graphics.Viewport.Height - buttonSprite.Height) / 2);
        toMenuButton.AddComponent(new ButtonComponent(
            buttonSprite.Width, 
            buttonSprite.Height,
            () => _sceneManager.Replace(new MenuScene(_content, _graphics, _sceneManager))));
        AddEntity(toMenuButton);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Begin();
        base.Draw(spriteBatch);
        spriteBatch.End();
    }
}