using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TryToEscape.Components;
using TryToEscape.Core;
using TryToEscape.Rendering;
using TryToEscape.World;

namespace TryToEscape.Scenes;

public class GameScene : Scene
{
    private MazeRenderer _mazeRenderer;
    private FogOfWarRenderer _fogRenderer;
    private Camera _camera;
    private Entity _player;
    private SceneManager _sceneManager;
    private const int TileSize = 16;
    

    public GameScene(ContentManager contentManager, GraphicsDevice graphicsDevice, SceneManager sceneManager)
    {

        _camera = new Camera(graphicsDevice.Viewport.Width, graphicsDevice.Viewport.Height);
        _sceneManager = sceneManager;

        var playerSprite = contentManager.Load<Texture2D>("assets/sprite");
        var keySprite = contentManager.Load<Texture2D>("assets/key");

        var generator = new MazeGenerator();
        var maze = generator.Generate(50, 30, 8, 4, 1);
        var start = generator.GetStartPosition();
        var keyPos = generator.GetRandomFloorPosition();

        var fog = new FogOfWar(maze);
        var pixel = new Texture2D(graphicsDevice, 1, 1);
        pixel.SetData(new[] { Color.White });
        _fogRenderer = new FogOfWarRenderer(fog, pixel, TileSize);
        
        _player = new Entity();
        _player.AddComponent(new SpriteComponent(playerSprite));
        _player.AddComponent(new InputComponent(100));
        _player.Position = new Vector2(start.X * TileSize, start.Y * TileSize);
        _player.AddComponent(new ColliderComponent(maze, TileSize, TileSize));
        _player.AddComponent(new FogOfWarUpdaterComponent(fog, TileSize, 30));
        _player.AddComponent(new InventoryComponent()); 
        _player.AddComponent(new ExitDetectorComponent(maze, TileSize, () => _sceneManager.ChangeScene(new MenuScene())));
        AddEntity(_player);

        var key = new Entity();
        key.AddComponent(new SpriteComponent(keySprite));
        key.Position = new Vector2(keyPos.X * TileSize, keyPos.Y * TileSize);
        key.AddComponent(new KeyComponent(_player, this, TileSize));
        AddEntity(key);

        var floorTexture = contentManager.Load<Texture2D>("assets/tiles/floor");
        var wallTexture  = contentManager.Load<Texture2D>("assets/tiles/wall");
        var exitTexture = contentManager.Load<Texture2D>("assets/tiles/exit");
        _mazeRenderer = new MazeRenderer(maze, floorTexture, wallTexture, exitTexture, TileSize);
    }

    public override Matrix GetCameraMatrix()
    {
        return _camera.GetTransformMatrix();
    }

    public override void Update(GameTime gameTime)
    {
        _camera.Follow(_player.Position);
        base.Update(gameTime);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {   
        var visibleArea = _camera.GetVisibleArea();
        _mazeRenderer.Draw(spriteBatch, visibleArea);
        _fogRenderer.Draw(spriteBatch, visibleArea);
        base.Draw(spriteBatch);
    }
}