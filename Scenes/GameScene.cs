using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TryToEscape.Components;
using TryToEscape.Config;
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
    private SpriteFont _hudFont;

    

    public GameScene(ContentManager contentManager, GraphicsDevice graphicsDevice, SceneManager sceneManager)
    {
        _content = contentManager;
        _graphics = graphicsDevice;
        _camera = new Camera(graphicsDevice.Viewport.Width, graphicsDevice.Viewport.Height, GameConfig.Zoom);
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

        spriteBatch.Begin();
        DrawHud(spriteBatch);
        spriteBatch.End();
    }

    private void InitializeScene()
    {
        var generator = new MazeGenerator();
        var maze = generator.Generate(GameConfig.MazeWidth, GameConfig.MazeHeight, GameConfig.MinLeafSize, GameConfig.MinRoomSize, GameConfig.Padding);
        var fog = new FogOfWar(maze);
        _hudFont = _content.Load<SpriteFont>("assets/fonts/pixel");

        InitializeRenderers(maze, fog);
        CreatePlayer(maze, generator, fog);
        CreateKeys(generator, fog);
        CreatePatrols(maze, generator, fog);
    }

    private void InitializeRenderers(Maze maze, FogOfWar fog)
    {
        _pixel = new Texture2D(_graphics, 1, 1);
        _pixel.SetData(new[] { Color.White });
        _fogRenderer = new FogOfWarRenderer(fog, _pixel, GameConfig.TileSize);

        var atlas = _content.Load<Texture2D>("assets/tiles/tileset");
        var defaultWallRect = new Rectangle(0,   0, 16, 16);
        var exitRect = new Rectangle(32, 128, 16, 16);
        var wallRects = BuildWallRects();

        var floorRects = BuildFloorRects();
        var defaultFloor = new Rectangle(150, 0, 16, 16);
        
        _mazeRenderer = new MazeRenderer(
            maze, atlas,
            floorRects, defaultFloor,
            wallRects, defaultWallRect,
            exitRect,
            GameConfig.TileSize);
            }

    private Dictionary<int, Rectangle[]> BuildWallRects()
    {
        return new Dictionary<int, Rectangle[]>
        {
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

            { 15,  new[] { new Rectangle(16, 0, 16, 16) } },

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

    private void DrawHud(SpriteBatch spriteBatch)
    {
        var inv = _player.GetComponent<InventoryComponent>();
        var text = $"Keys: {inv.KeyCount}/{GameConfig.KeyToExit}";
        spriteBatch.DrawString(
            _hudFont, 
            text, 
            new Vector2(60, 40), 
            Color.White,
            rotation: 0f,
            origin: Vector2.Zero,
            scale: 1f,
            effects: SpriteEffects.None,
            layerDepth: 0f);
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
        var sprite = _content.Load<Texture2D>("assets/characters/player");
        player.AddComponent(new AnimatedSpriteComponent(
            sprite, 
            frameWidth: 16, frameHeight: 16, 
            frameCount: 4,
            frameDuration: 0.12f,
            pauseWhenStill: true));
        player.AddComponent(new InputComponent(GameConfig.PlayerSpeed));
        player.Position = position * GameConfig.TileSize;
        player.AddComponent(new ColliderComponent(maze, GameConfig.TileSize, GameConfig.TileSize));
        player.AddComponent(new FogOfWarUpdaterComponent(fog, GameConfig.TileSize, GameConfig.FogRadius));
        player.AddComponent(new InventoryComponent());
        player.AddComponent(new ExitDetectorComponent(maze, GameConfig.KeyToExit, GameConfig.TileSize,
            () => _sceneManager.Replace(new VictoryScene(_content, _graphics, _sceneManager))));
        player.AddComponent(new PauseTriggerComponent(
            () => _sceneManager.Push(new PauseScene(_content, _graphics, _sceneManager))));

        AddEntity(player);
        _player = player;
    }

    private Entity CreatePatrol(Maze maze, MazeGenerator generator, FogOfWar fog)
    {
        var sprite = _content.Load<Texture2D>("assets/characters/patrol");
        var waypoints = generator.GetRandomRooms(GameConfig.WaypointsPerPatrol);
        var patrol = new Entity();

        patrol.AddComponent(new AnimatedSpriteComponent(sprite, 16, 16, 4, 0.12f, true));
        var indicatorSheet = _content.Load<Texture2D>("assets/characters/exclamation");
        patrol.AddComponent(new PatrolIndicatorComponent(
            indicatorSheet,
            frameWidth: 16, frameHeight: 16,
            frameCount: 4,
            frameDuration: 0.15f,
            offset: new Vector2(0, -16)));
        patrol.AddComponent(new FogVisibilityComponent(fog, GameConfig.TileSize));
        patrol.AddComponent(new VisionComponent(
            maze, _player, GameConfig.TileSize, GameConfig.PatrolVisionRadius, GameConfig.PatrolVisionAngle, GameConfig.GracePeriod, _pixel,
            () => {}));
        patrol.Position = waypoints[0].ToPixel(GameConfig.TileSize);
        patrol.AddComponent(new PatrolMovementComponent(maze, waypoints, GameConfig.TileSize, GameConfig.PatrolSpeed, GameConfig.PatrolChaseSpeed, () => _sceneManager.Replace(new DefeatScene(_content, _graphics, _sceneManager))));

        return patrol;
    }

    private void CreatePatrols(Maze maze, MazeGenerator generator, FogOfWar fog)
    {
        for (int i = 0; i < GameConfig.PatrolCount; i++)
            AddEntity(CreatePatrol(maze, generator, fog));
    }

    private void CreateKeys(MazeGenerator generator, FogOfWar fog)
    {
        var positions = generator.GetRandomRooms(GameConfig.KeyCount);
        foreach (var pos in positions)
            AddEntity(CreateKey(pos, fog));
    }

    private Entity CreateKey(Point position, FogOfWar fog)
    {
        var key = new Entity();
        var sprite = _content.Load<Texture2D>("assets/items/key");
        key.AddComponent(new AnimatedSpriteComponent(
            sprite, 
            frameWidth: 16, frameHeight: 16, 
            frameCount: 4, 
            frameDuration: 0.15f));
        key.AddComponent(new FogVisibilityComponent(fog, GameConfig.TileSize));
        key.Position = position.ToPixel(GameConfig.TileSize);
        key.AddComponent(new KeyComponent(_player, this, GameConfig.TileSize));

        return key;
    }
}