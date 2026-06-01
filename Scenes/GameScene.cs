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

    private const int TileSize = 16;
    private const int PatrolCount = 10;
    private const int WaypointsPerPatrol = 3;
    private const int PlayerSpeed = 150;
    private const int FogRadius = 6;
    private const int PatrolVisionRadius = 8;
    private const int PatrolSpeed = 60;
    private const int PatrolVisionAngle = 30;
    private const float Zoom = 5f;
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
        spriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: _camera.GetTransformMatrix());

        var visibleArea = _camera.GetVisibleArea();
        _mazeRenderer.Draw(spriteBatch, visibleArea);
        _fogRenderer.Draw(spriteBatch, visibleArea);
        base.Draw(spriteBatch);

        spriteBatch.End();
    }

    private void InitializeScene()
    {
        var generator = new MazeGenerator();
        var maze = generator.Generate(100, 60, 8, 4, 1);
        var fog = new FogOfWar(maze);

        InitializeRenderers(maze, fog);
        CreatePlayer(maze, generator, fog);
        CreateKey(generator, fog);
        CreatePatrols(maze, generator, fog);
    }

    private void InitializeRenderers(Maze maze, FogOfWar fog)
    {
        _pixel = new Texture2D(_graphics, 1, 1);
        _pixel.SetData(new[] { Color.White });
        _fogRenderer = new FogOfWarRenderer(fog, _pixel, TileSize);

        var atlas = _content.Load<Texture2D>("assets/tiles/tileset");
        var defaultWallRect = new Rectangle(0,   0, 16, 16);
        var exitRect        = new Rectangle(0, 150, 16, 16);
        var wallRects = BuildWallRects();

        var floorRects = BuildFloorRects();
        var defaultFloor = new Rectangle(150, 0, 16, 16);

        _mazeRenderer = new MazeRenderer(
            maze, atlas,
            floorRects, defaultFloor,
            wallRects, defaultWallRect,
            exitRect,
            TileSize);
            }

    private Dictionary<int, Rectangle[]> BuildWallRects()
    {
        return new Dictionary<int, Rectangle[]>
        {
            // Углы
            { 16,  new[] { new Rectangle(80, 0, 16, 16) } },
            { 17,  new[] { new Rectangle(0, 0, 16, 16) } },
            { 18,  new[] { new Rectangle(80, 64, 16, 16) } },
            { 19, new[] { new Rectangle(0, 64, 16, 16) } },

            { 2,  new[] {
                new Rectangle(0, 16, 16, 16),
                new Rectangle(0, 32, 16, 16),
                new Rectangle(0, 48, 16, 16)
            }},

            { 8,  new[] {
                new Rectangle(80, 16, 16, 16),
                new Rectangle(80, 32, 16, 16),
                new Rectangle(80, 48, 16, 16)
            }},

            { 10, new[] {new Rectangle(64, 96, 16, 16)}},
            { 14, new[] {new Rectangle(16, 0, 16, 16)}},
            { 11, new[] {new Rectangle(64, 96, 16, 16)}},
            { 9, new[] {new Rectangle(0, 80, 16, 16)}},
            { 3, new[] {new Rectangle(48, 80, 16, 16)}},

            { 5, new[] {
                new Rectangle(16, 0, 16, 16),
                new Rectangle(32, 0, 16, 16),
                new Rectangle(48, 0, 16, 16),
                new Rectangle(64, 0, 16, 16)
            }},

            { 7, new[] {
                new Rectangle(16, 0, 16, 16),
                new Rectangle(32, 0, 16, 16),
                new Rectangle(48, 0, 16, 16),
                new Rectangle(64, 0, 16, 16)
            }},

            { 13, new[] {
                new Rectangle(16, 0, 16, 16),
                new Rectangle(32, 0, 16, 16),
                new Rectangle(48, 0, 16, 16),
                new Rectangle(64, 0, 16, 16)
            }},

            { 6, new[] {
                new Rectangle(16, 0, 16, 16),
                new Rectangle(32, 0, 16, 16),
                new Rectangle(48, 0, 16, 16),
                new Rectangle(64, 0, 16, 16)
            }},

            { 12, new[] {
                new Rectangle(16, 0, 16, 16),
                new Rectangle(32, 0, 16, 16),
                new Rectangle(48, 0, 16, 16),
                new Rectangle(64, 0, 16, 16)
            }},

            { 4, new[] {
                new Rectangle(16, 0, 16, 16),
                new Rectangle(32, 0, 16, 16),
                new Rectangle(48, 0, 16, 16),
                new Rectangle(64, 0, 16, 16)
            }},

            { 1, new[] {
                new Rectangle(16, 64, 16, 16),
                new Rectangle(32, 64, 16, 16),
                new Rectangle(48, 64, 16, 16),
                new Rectangle(64, 64, 16, 16)
            }},

            { 0, new[] { new Rectangle(128, 112, 16, 16) } },
        };
    }

    private Dictionary<int, Rectangle[]> BuildFloorRects()
    {
        return new Dictionary<int, Rectangle[]>
        {
            { 0, new[] {
                new Rectangle(96, 0, 16, 16),
                new Rectangle(112, 0, 16, 16),
                new Rectangle(128, 0, 16, 16),
                new Rectangle(144, 0, 16, 16),
                new Rectangle(96, 16, 16, 16),
                new Rectangle(112, 16, 16, 16),
                new Rectangle(128, 16, 16, 16),
                new Rectangle(144, 16, 16, 16),
                new Rectangle(96, 32, 16, 16),
                new Rectangle(112, 32, 16, 16),
                new Rectangle(128, 32, 16, 16),
                new Rectangle(144, 32, 16, 16)
            }},

            { 1, new[] {
                new Rectangle(32, 16, 16, 16),
                new Rectangle(48, 16, 16, 16)
            }},

            { 2, new[] {
                new Rectangle(64, 32, 16, 16),
            }},

            { 8, new[] {
                new Rectangle(16, 32, 16, 16),
            }},

            { 4, new[] {
                new Rectangle(32, 48, 16, 16),
                new Rectangle(48, 48, 16, 16)
            }},

            { 9, new[] {
                new Rectangle(16, 16, 16, 16),
            }},

            { 3, new[] {
                new Rectangle(64, 16, 16, 16),
            }},

            { 12, new[] {
                new Rectangle(16, 48, 16, 16),
            }},

            { 6, new[] {
                new Rectangle(64, 48, 16, 16),
            }},
        };
    }

    private void CreatePlayer(Maze maze, MazeGenerator generator, FogOfWar fog)
    {
        var position = generator.GetStartPosition();
        var player = new Entity();
        var sprite = _content.Load<Texture2D>("assets/player");
        player.AddComponent(new AnimatedSpriteComponent(
            sprite, 
            frameWidth: 16, frameHeight: 16, 
            frameCount: 4,
            frameDuration: 0.12f,
            pauseWhenStill: true));
        player.AddComponent(new InputComponent(PlayerSpeed));
        player.Position = position * TileSize;
        player.AddComponent(new ColliderComponent(maze, TileSize, TileSize));
        player.AddComponent(new FogOfWarUpdaterComponent(fog, TileSize, FogRadius));
        player.AddComponent(new InventoryComponent());
        player.AddComponent(new ExitDetectorComponent(maze, TileSize,
            () => _sceneManager.Replace(new VictoryScene(_content, _graphics, _sceneManager))));
        player.AddComponent(new PauseTriggerComponent(
            () => _sceneManager.Push(new PauseScene(_content, _graphics, _sceneManager))));

        AddEntity(player);
        _player = player;
    }

    private Entity CreatePatrol(Maze maze, MazeGenerator generator, FogOfWar fog)
    {
        var sprite = _content.Load<Texture2D>("assets/skeleton");
        var waypoints = generator.GetRandomRooms(WaypointsPerPatrol);
        var patrol = new Entity();

        patrol.AddComponent(new AnimatedSpriteComponent(sprite, 16, 16, 4, 0.12f, true));
        patrol.AddComponent(new FogVisibilityComponent(fog, TileSize));
        patrol.AddComponent(new VisionComponent(
            maze, _player, TileSize, PatrolVisionRadius, PatrolVisionAngle, GracePeriod, _pixel,
            () => _sceneManager.Replace(new DefeatScene(_content, _graphics, _sceneManager))));
        patrol.Position = waypoints[0].ToPixel(TileSize);
        patrol.AddComponent(new PatrolMovementComponent(maze, waypoints, TileSize, PatrolSpeed));

        return patrol;
    }

    private void CreatePatrols(Maze maze, MazeGenerator generator, FogOfWar fog)
    {
        for (int i = 0; i < PatrolCount; i++)
            AddEntity(CreatePatrol(maze, generator, fog));
    }

    private void CreateKey(MazeGenerator generator, FogOfWar fog)
    {
        var position = generator.GetRandomFloorPosition();
        var key = new Entity();
        var sprite = _content.Load<Texture2D>("assets/key");
        key.AddComponent(new AnimatedSpriteComponent(
            sprite, 
            frameWidth: 16, frameHeight: 16, 
            frameCount: 4, 
            frameDuration: 0.15f));
        key.AddComponent(new FogVisibilityComponent(fog, TileSize));
        key.Position = position.ToPixel(TileSize);
        key.AddComponent(new KeyComponent(_player, this, TileSize));

        AddEntity(key);
    }
}