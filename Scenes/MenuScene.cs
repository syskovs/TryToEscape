using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TryToEscape.Components;
using TryToEscape.Core;

namespace TryToEscape.Scenes;

public class MenuScene : Scene
{
    private ContentManager _content;
    private GraphicsDevice _graphics;
    private SceneManager _sceneManager;
    private Texture2D _background;

    public MenuScene(ContentManager content, GraphicsDevice graphics, SceneManager sceneManager)
    {
        _content = content;
        _graphics = graphics;
        _sceneManager = sceneManager;
         
        _background = content.Load<Texture2D>("assets/scenes/backgrounds/menu");
        var buttonSprite = _content.Load<Texture2D>("assets/scenes/buttons/play");
        
        var newGameButton = new Entity();
        newGameButton.AddComponent(new SpriteComponent(buttonSprite));
        newGameButton.Position = new Vector2(
            (_graphics.Viewport.Width  - buttonSprite.Width)  / 2,
            (_graphics.Viewport.Height - buttonSprite.Height) / 2);
        newGameButton.AddComponent(new ButtonComponent(
            buttonSprite.Width, 
            buttonSprite.Height,
            () => _sceneManager.Replace(new GameScene(_content, _graphics, _sceneManager))));
        AddEntity(newGameButton);

        var exitSprite = _content.Load<Texture2D>("assets/scenes/buttons/exit");
        var exitButton = new Entity();

        exitButton.AddComponent(new SpriteComponent(exitSprite));
        exitButton.Position = new Vector2(
            (_graphics.Viewport.Width  - exitSprite.Width)  / 2,
            (_graphics.Viewport.Height - exitSprite.Height) / 2 + 450);
        exitButton.AddComponent(new ButtonComponent(
            exitSprite.Width, 
            exitSprite.Height,
            () => Environment.Exit(0)));
        AddEntity(exitButton);
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