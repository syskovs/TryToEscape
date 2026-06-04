using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TryToEscape.Components;
using TryToEscape.Core;

namespace TryToEscape.Scenes;

public class PauseScene : Scene
{
    private SceneManager _sceneManager;
    private Texture2D _pixel;
    private GraphicsDevice _graphics;
    private ContentManager _content;
    private Texture2D _background;

    public PauseScene(ContentManager content, GraphicsDevice graphics, SceneManager sceneManager)
    {
        _graphics = graphics;
        _sceneManager = sceneManager;
        _content = content;

        _background = content.Load<Texture2D>("assets/scenes/backgrounds/pause");

        var resumeSprite = content.Load<Texture2D>("assets/scenes/buttons/resume");
        var toMenuSprite = content.Load<Texture2D>("assets/scenes/buttons/menu");

        var resumeButton = new Entity();
        resumeButton.AddComponent(new SpriteComponent(resumeSprite));
        resumeButton.Position = new Vector2(
            (_graphics.Viewport.Width  - resumeSprite.Width)  / 2,
            (_graphics.Viewport.Height - resumeSprite.Height) / 2);
        resumeButton.AddComponent(new ButtonComponent(
            resumeSprite.Width,
            resumeSprite.Height,
            () => _sceneManager.Pop()
        ));
        AddEntity(resumeButton);

        var toMenuButton = new Entity();
        toMenuButton.AddComponent(new SpriteComponent(toMenuSprite));
        toMenuButton.Position = new Vector2(
            (_graphics.Viewport.Width  - toMenuSprite.Width)  / 2,
            (_graphics.Viewport.Height - toMenuSprite.Height) / 2 + 400);
        toMenuButton.AddComponent(new ButtonComponent(
            toMenuSprite.Width, 
            toMenuSprite.Height,
            () => _sceneManager.Replace(new MenuScene(_content, _graphics, _sceneManager))));
        AddEntity(toMenuButton);

        _pixel = new Texture2D(graphics, 1, 1);
        _pixel.SetData(new[] { Color.White });

        var trigger = new Entity();
        trigger.AddComponent(new PauseTriggerComponent(() => _sceneManager.Pop()));
        AddEntity(trigger);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Begin();
        
        spriteBatch.Draw(_pixel, 
            new Rectangle(0, 0, _graphics.Viewport.Width, _graphics.Viewport.Height), 
            Color.Black * 0.5f);

        spriteBatch.Draw(_background,
            new Rectangle((_graphics.Viewport.Width  - _background.Width)  / 2, 100, _background.Width, _background.Height), 
            Color.White);
        base.Draw(spriteBatch);
        spriteBatch.End();
    }
}