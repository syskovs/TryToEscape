using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TryToEscape.Components;
using TryToEscape.Config;
using TryToEscape.Core;

namespace TryToEscape.Scenes;

public class DefeatScene : Scene
{
    private Texture2D _background;
    private GameContext _context;

    public DefeatScene(GameContext context)
    {
        _context = context;

        _background = _context.Content.Load<Texture2D>(Assets.DefeatBackground);
        var menuButtonSprite = _context.Content.Load<Texture2D>(Assets.MenuButton);
        
        var menuButton = new Entity();
        menuButton.AddComponent(new SpriteComponent(menuButtonSprite));
        menuButton.Position = new Vector2(
            (_context.Graphics.Viewport.Width  - menuButtonSprite.Width)  / 2,
            (_context.Graphics.Viewport.Height - menuButtonSprite.Height) / 2 + _context.Graphics.Viewport.Height / 8);
        menuButton.AddComponent(new ButtonComponent(
            menuButtonSprite.Width, 
            menuButtonSprite.Height,
            () => _context.Scenes.Replace(new MenuScene(_context))));
        AddEntity(menuButton);
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