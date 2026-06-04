using Microsoft.Xna.Framework;
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
    private Entity _player;
    private SpriteFont _hudFont;
    private readonly GameContext _context;

    public GameScene(GameContext ctx)
    {
        _context = ctx;
        _camera = new Camera(ctx.Graphics.Viewport.Width, ctx.Graphics.Viewport.Height, GameConfig.Zoom);
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
        var maze = generator.Generate(
            GameConfig.MazeWidth, GameConfig.MazeHeight,
            GameConfig.MinLeafSize, GameConfig.MinRoomSize, GameConfig.Padding);
        var fog = new FogOfWar(maze);
        _hudFont = _context.Content.Load<SpriteFont>(Assets.PixelFont);

        InitializeRenderers(maze, fog);

        _player = _context.Entities.Player(maze, fog, generator.GetStartPosition(),
            onVictory: () => _context.Scenes.Replace(new VictoryScene(_context)),
            onPause:   () => _context.Scenes.Push(new PauseScene(_context)));
        AddEntity(_player);

        foreach (var pos in generator.GetRandomRooms(GameConfig.KeyCount))
            AddEntity(_context.Entities.Key(fog, _player, this, pos));

        for (int i = 0; i < GameConfig.PatrolCount; i++)
        {
            var waypoints = generator.GetRandomRooms(GameConfig.WaypointsPerPatrol);
            AddEntity(_context.Entities.Patrol(maze, fog, _context.Pixel, _player, waypoints,
                onCatch: () => _context.Scenes.Replace(new DefeatScene(_context))));
        }
    }

    private void InitializeRenderers(Maze maze, FogOfWar fog)
    {
        _fogRenderer = new FogOfWarRenderer(fog, _context.Pixel, GameConfig.TileSize);

        var atlas = _context.Content.Load<Texture2D>(Assets.Tileset);
        _mazeRenderer = new MazeRenderer(
            maze, atlas,
            TilesetMapping.BuildFloorRects(), TilesetMapping.DefaultFloor,
            TilesetMapping.BuildWallRects(),  TilesetMapping.DefaultWall,
            TilesetMapping.Exit,
            GameConfig.TileSize);
    }

    private void DrawHud(SpriteBatch spriteBatch)
    {
        var inv = _player.GetComponent<InventoryComponent>();
        var text = $"Keys: {inv.KeyCount}/{GameConfig.KeyToExit}";
        spriteBatch.DrawString(_hudFont, text, new Vector2(60, 40), Color.White);
    }
}