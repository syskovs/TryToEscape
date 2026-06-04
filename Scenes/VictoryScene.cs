using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TryToEscape.Components;
using TryToEscape.Config;
using TryToEscape.Core;

namespace TryToEscape.Scenes;

public class VictoryScene : Scene
{
    private Texture2D _background;
    private GameContext _context;

    public VictoryScene(GameContext context)
    {
        _context = context;

        _background = _context.Content.Load<Texture2D>(Assets.VictoryBackground);
        var buttonSprite = _context.Content.Load<Texture2D>(Assets.MenuButton);
        
        var toMenuButton = new Entity();
        toMenuButton.AddComponent(new SpriteComponent(buttonSprite));
        toMenuButton.Position = new Vector2(
            (_context.Graphics.Viewport.Width  - buttonSprite.Width)  / 2,
            (_context.Graphics.Viewport.Height - buttonSprite.Height) / 2 + 200);
        toMenuButton.AddComponent(new ButtonComponent(
            buttonSprite.Width, 
            buttonSprite.Height,
            () => _context.Scenes.Replace(new MenuScene(_context))));
        AddEntity(toMenuButton);
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