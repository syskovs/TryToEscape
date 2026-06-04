using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TryToEscape.Components;
using TryToEscape.Config;
using TryToEscape.Core;
using TryToEscape.World;

namespace TryToEscape.Entities;

public class EntityFactory
{
    private readonly ContentManager _content;

    public EntityFactory(ContentManager content)
    {
        _content = content;
    }

    public Entity Player(
        Maze maze, FogOfWar fog,
        Vector2 startTile,
        Action onVictory, Action onPause)
    {
        var sprite = _content.Load<Texture2D>(Assets.PlayerSprite);
        var player = new Entity();

        player.Position = startTile * GameConfig.TileSize;
        player.AddComponent(new AnimatedSpriteComponent(sprite, 16, 16, 4, 0.12f, true));
        player.AddComponent(new InputComponent(GameConfig.PlayerSpeed));
        player.AddComponent(new ColliderComponent(maze, GameConfig.TileSize, GameConfig.TileSize));
        player.AddComponent(new FogOfWarUpdaterComponent(fog, GameConfig.TileSize, GameConfig.FogRadius));
        player.AddComponent(new InventoryComponent());
        player.AddComponent(new ExitDetectorComponent(maze, GameConfig.KeyToExit, GameConfig.TileSize, onVictory));
        player.AddComponent(new PauseTriggerComponent(onPause));

        return player;
    }

    public Entity Patrol(
        Maze maze, FogOfWar fog, Texture2D pixel,
        Entity player,
        IReadOnlyList<Point> waypoints,
        Action onCatch)
    {
        var sprite = _content.Load<Texture2D>(Assets.PatrolSprite);
        var indicatorSheet = _content.Load<Texture2D>(Assets.ExclamationMark);
        var patrol = new Entity();

        patrol.Position = waypoints[0].ToPixel(GameConfig.TileSize);
        patrol.AddComponent(new AnimatedSpriteComponent(sprite, 16, 16, 4, 0.12f, true));
        patrol.AddComponent(new PatrolIndicatorComponent(indicatorSheet, 16, 16, 4, 0.15f, new Vector2(0, -16)));
        patrol.AddComponent(new FogVisibilityComponent(fog, GameConfig.TileSize));
        patrol.AddComponent(new VisionComponent(
            maze, 
            player,
            GameConfig.TileSize,
            GameConfig.PatrolVisionRadius,
            GameConfig.PatrolVisionAngle,
            GameConfig.GracePeriod,
            pixel));
        patrol.AddComponent(new PatrolMovementComponent(
            maze, 
            waypoints, 
            GameConfig.TileSize,
            GameConfig.PatrolSpeed,
            GameConfig.PatrolChaseSpeed,
            onCatch));

        return patrol;
    }

    public Entity Key(FogOfWar fog, Entity player, Scene scene, Point tile)
    {
        var sprite = _content.Load<Texture2D>(Assets.KeySprite);
        var key = new Entity();

        key.Position = tile.ToPixel(GameConfig.TileSize);
        key.AddComponent(new AnimatedSpriteComponent(sprite, 16, 16, 4, 0.12f));
        key.AddComponent(new FogVisibilityComponent(fog, GameConfig.TileSize));
        key.AddComponent(new KeyComponent(player, scene, GameConfig.TileSize));

        return key;
    }
}