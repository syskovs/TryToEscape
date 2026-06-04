using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TryToEscape.Components;
using TryToEscape.Config;
using TryToEscape.Core;

namespace TryToEscape.Scenes;

public class PauseScene : Scene
{
    private Texture2D _background;
    private GameContext _context;
    

    public PauseScene(GameContext context)
    {
        _context = context;

        _background = _context.Content.Load<Texture2D>(Assets.PauseBackground);
        var resumeButtomSprite = _context.Content.Load<Texture2D>(Assets.ResumeButton);
        var menuButtonSprite = _context.Content.Load<Texture2D>(Assets.MenuButton);

        var resumeButton = new Entity();
        resumeButton.AddComponent(new SpriteComponent(resumeButtomSprite));
        resumeButton.Position = new Vector2(
            (_context.Graphics.Viewport.Width  - resumeButtomSprite.Width)  / 2,
            (_context.Graphics.Viewport.Height - resumeButtomSprite.Height) / 2);
        resumeButton.AddComponent(new ButtonComponent(
            resumeButtomSprite.Width,
            resumeButtomSprite.Height,
            () => _context.Scenes.Pop()
        ));
        AddEntity(resumeButton);

        var menuButton = new Entity();
        menuButton.AddComponent(new SpriteComponent(menuButtonSprite));
        menuButton.Position = new Vector2(
            (_context.Graphics.Viewport.Width  - menuButtonSprite.Width)  / 2,
            (_context.Graphics.Viewport.Height - menuButtonSprite.Height) / 2 + _context.Graphics.Viewport.Height / 4);
        menuButton.AddComponent(new ButtonComponent(
            menuButtonSprite.Width, 
            menuButtonSprite.Height,
            () => _context.Scenes.Replace(new MenuScene(_context))));
        AddEntity(menuButton);

        var trigger = new Entity();
        trigger.AddComponent(new PauseTriggerComponent(() => _context.Scenes.Pop()));
        AddEntity(trigger);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Begin();
        
        spriteBatch.Draw(_context.Pixel, 
            new Rectangle(0, 0, _context.Graphics.Viewport.Width, _context.Graphics.Viewport.Height), 
            Color.Black * 0.5f);

        spriteBatch.Draw(_background,
            new Rectangle(
                (_context.Graphics.Viewport.Width  - _background.Width)  / 2, 
                _context.Graphics.Viewport.Height / 16, 
                _background.Width, _background.Height), 
            Color.White);
        base.Draw(spriteBatch);
        spriteBatch.End();
    }
}