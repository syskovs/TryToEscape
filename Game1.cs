using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TryToEscape.Core;
using TryToEscape.Rendering;
using TryToEscape.Scenes;

namespace TryToEscape;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private SceneManager _sceneManager;
    private Color _color = new Color(0x25, 0x13, 0x1a);

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        Window.IsBorderless = true;
        Window.Position = Point.Zero;
        _graphics.PreferredBackBufferWidth = GraphicsDevice.DisplayMode.Width;
        _graphics.PreferredBackBufferHeight = GraphicsDevice.DisplayMode.Height;
        _graphics.ApplyChanges();
        
        _sceneManager = new SceneManager();
        var context = new GameContext(Content, GraphicsDevice, _sceneManager);
        _sceneManager.Replace(new MenuScene(context));
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
    }

    protected override void Update(GameTime gameTime)
    {
        _sceneManager.PreUpdate(gameTime);
        _sceneManager.Update(gameTime);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(_color);
        _sceneManager.Draw(_spriteBatch);
        base.Draw(gameTime);
    }
}
