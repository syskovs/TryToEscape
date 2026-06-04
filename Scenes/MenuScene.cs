using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TryToEscape.Components;
using TryToEscape.Config;
using TryToEscape.Core;

namespace TryToEscape.Scenes;

public class MenuScene : Scene
{
    private Texture2D _background;
    private GameContext _context;

    public MenuScene(GameContext context)
    {
        _context = context;
         
        _background = _context.Content.Load<Texture2D>(Assets.MenuBackground);
        var playButtonSprite = _context.Content.Load<Texture2D>(Assets.PlayButton);
        var exitButtonSprite = _context.Content.Load<Texture2D>(Assets.ExitButton);
        
        var playButton = new Entity();
        playButton.AddComponent(new SpriteComponent(playButtonSprite));
        playButton.Position = new Vector2(
            (_context.Graphics.Viewport.Width  - playButtonSprite.Width)  / 2,
            (_context.Graphics.Viewport.Height - playButtonSprite.Height) / 2);
        playButton.AddComponent(new ButtonComponent(
            playButtonSprite.Width, 
            playButtonSprite.Height,
            () => _context.Scenes.Replace(new GameScene(_context))));
        AddEntity(playButton);

        var exitButton = new Entity();
        exitButton.AddComponent(new SpriteComponent(exitButtonSprite));
        exitButton.Position = new Vector2(
            (_context.Graphics.Viewport.Width  - exitButtonSprite.Width)  / 2,
            (_context.Graphics.Viewport.Height - exitButtonSprite.Height) / 2 + _context.Graphics.Viewport.Height / 4);
        exitButton.AddComponent(new ButtonComponent(
            exitButtonSprite.Width, 
            exitButtonSprite.Height,
            () => Environment.Exit(0)));
        AddEntity(exitButton);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Begin();
        spriteBatch.Draw(_background, 
            new Rectangle(0, 0, _context.Graphics.Viewport.Width, _context.Graphics.Viewport.Height), 
            Color.White);
        base.Draw(spriteBatch);
        spriteBatch.End();
    }
}