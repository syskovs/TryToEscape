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

        var resumeSprite = _context.Content.Load<Texture2D>(Assets.ResumeButton);
        var toMenuSprite = _context.Content.Load<Texture2D>(Assets.MenuButton);

        var resumeButton = new Entity();
        resumeButton.AddComponent(new SpriteComponent(resumeSprite));
        resumeButton.Position = new Vector2(
            (_context.Graphics.Viewport.Width  - resumeSprite.Width)  / 2,
            (_context.Graphics.Viewport.Height - resumeSprite.Height) / 2);
        resumeButton.AddComponent(new ButtonComponent(
            resumeSprite.Width,
            resumeSprite.Height,
            () => _context.Scenes.Pop()
        ));
        AddEntity(resumeButton);

        var toMenuButton = new Entity();
        toMenuButton.AddComponent(new SpriteComponent(toMenuSprite));
        toMenuButton.Position = new Vector2(
            (_context.Graphics.Viewport.Width  - toMenuSprite.Width)  / 2,
            (_context.Graphics.Viewport.Height - toMenuSprite.Height) / 2 + 400);
        toMenuButton.AddComponent(new ButtonComponent(
            toMenuSprite.Width, 
            toMenuSprite.Height,
            () => _context.Scenes.Replace(new MenuScene(_context))));
        AddEntity(toMenuButton);

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
            new Rectangle((_context.Graphics.Viewport.Width  - _background.Width)  / 2, 100, _background.Width, _background.Height), 
            Color.White);
        base.Draw(spriteBatch);
        spriteBatch.End();
    }
}