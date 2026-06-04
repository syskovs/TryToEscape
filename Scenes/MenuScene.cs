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
        var buttonSprite = _context.Content.Load<Texture2D>(Assets.PlayButton);
        
        var newGameButton = new Entity();
        newGameButton.AddComponent(new SpriteComponent(buttonSprite));
        newGameButton.Position = new Vector2(
            (_context.Graphics.Viewport.Width  - buttonSprite.Width)  / 2,
            (_context.Graphics.Viewport.Height - buttonSprite.Height) / 2);
        newGameButton.AddComponent(new ButtonComponent(
            buttonSprite.Width, 
            buttonSprite.Height,
            () => _context.Scenes.Replace(new GameScene(_context))));
        AddEntity(newGameButton);

        var exitSprite = _context.Content.Load<Texture2D>(Assets.ExitButton);
        var exitButton = new Entity();

        exitButton.AddComponent(new SpriteComponent(exitSprite));
        exitButton.Position = new Vector2(
            (_context.Graphics.Viewport.Width  - exitSprite.Width)  / 2,
            (_context.Graphics.Viewport.Height - exitSprite.Height) / 2 + 450);
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
            new Rectangle(0, 0, _context.Graphics.Viewport.Width, _context.Graphics.Viewport.Height), 
            Color.White);
        base.Draw(spriteBatch);
        spriteBatch.End();
    }
}