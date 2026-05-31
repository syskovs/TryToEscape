using System.Collections.Generic;
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
    private ContentManager _content;
    private GraphicsDevice _graphics;
    private SceneManager _sceneManager;
    private Entity _player;
    private Texture2D _pixel;
    
    private const int TileSize = 32;
    private const int PatrolCount = 1;
    private const int WaypointsPerPatrol = 3;
    private const int PlayerSpeed = 300;
    private const int FogRadius = 8;
    private const int PatrolVisionRadius = 8;
    private const int PatrolSpeed = 60;
    private const int PatrolVisionAngle = 30;
    private const float Zoom = 2f;
    private const float GracePeriod = 1f;

    public GameScene(ContentManager contentManager, GraphicsDevice graphicsDevice, SceneManager sceneManager)
    {
        _content = contentManager;
        _graphics = graphicsDevice;
        _camera = new Camera(graphicsDevice.Viewport.Width, graphicsDevice.Viewport.Height, Zoom);
        _sceneManager = sceneManager;

        InitializeScene();
    }

    public override void Update(GameTime gameTime)
    {
        _camera.Follow(_player.Position);
        base.Update(gameTime);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {   
        spriteBatch.Begin(transformMatrix: _camera.GetTransformMatrix());
    
        var visibleArea = _camera.GetVisibleArea();
        _mazeRenderer.Draw(spriteBatch, visibleArea);
        _fogRenderer.Draw(spriteBatch, visibleArea);
        base.Draw(spriteBatch);
    
        spriteBatch.End();
    }

    private void InitializeScene()
    {
        var generator = new MazeGenerator();
        var maze = generator.Generate(50, 30, 8, 4, 1);
        var fog = new FogOfWar(maze);

        InitializeRenderers(maze, fog);
        CreatePlayer(maze, generator, fog);
        CreateKey(generator);
        CreatePatrols(maze, generator);
    }
    private void InitializeRenderers(Maze maze, FogOfWar fog)
    {
        _pixel = new Texture2D(_graphics, 1, 1);
        _pixel.SetData(new[] { Color.White });

        _fogRenderer = new FogOfWarRenderer(fog, _pixel, TileSize);

        var floorTexture = _content.Load<Texture2D>("assets/tiles/floor");
        var wallTexture = _content.Load<Texture2D>("assets/tiles/wall");
        var exitTexture = _content.Load<Texture2D>("assets/tiles/exit");

        _mazeRenderer = new MazeRenderer(maze, floorTexture, wallTexture, exitTexture, TileSize);
    }

    private void CreatePlayer(Maze maze, MazeGenerator generator, FogOfWar fog)
    {
        var sprite = _content.Load<Texture2D>("assets/sprite");
        var position = generator.GetStartPosition();
        var player = new Entity();

        player.AddComponent(new SpriteComponent(sprite));
        player.AddComponent(new InputComponent(PlayerSpeed));
        player.Position = position * TileSize;
        player.AddComponent(new ColliderComponent(maze, TileSize, TileSize));
        player.AddComponent(new FogOfWarUpdaterComponent(fog, TileSize, FogRadius));
        player.AddComponent(new InventoryComponent()); 
        player.AddComponent(new ExitDetectorComponent(maze, TileSize, () => _sceneManager.Replace(new VictoryScene(_content, _graphics, _sceneManager))));
        player.AddComponent(new PauseTriggerComponent(() => _sceneManager.Push(new PauseScene(_content, _graphics, _sceneManager))));
        AddEntity(player);
        _player = player;
    }

    private Entity CreatePatrol(Maze maze, MazeGenerator generator)
    {
        var sprite = _content.Load<Texture2D>("assets/sprite");
        var waypoints = generator.GetRandomRooms(WaypointsPerPatrol);
        var patrol = new Entity();

        patrol.AddComponent(new SpriteComponent(sprite));
        patrol.Position = waypoints[0].ToPixel(TileSize);
        patrol.AddComponent(new PatrolMovementComponent(maze,waypoints, TileSize, PatrolSpeed));
        patrol.AddComponent(new VisionComponent(maze, _player, TileSize, PatrolVisionRadius, PatrolVisionAngle, GracePeriod, _pixel, () => _sceneManager.Replace(new DefeatScene(_content, _graphics, _sceneManager))));

        return patrol;
    }

    private void CreatePatrols(Maze maze, MazeGenerator generator)
    {
        for (int i = 0; i < PatrolCount; i++)
        {
            AddEntity(CreatePatrol(maze, generator));
        }
    }

    private void CreateKey(MazeGenerator generator)
    {
        var position = generator.GetRandomFloorPosition();
        var sprite = _content.Load<Texture2D>("assets/key");
        var key = new Entity();

        key.AddComponent(new SpriteComponent(sprite));
        key.Position = new Vector2(position.X * TileSize, position.Y * TileSize);
        key.AddComponent(new KeyComponent(_player, this, TileSize));

        AddEntity(key);
    }
}