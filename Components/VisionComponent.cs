using System;
using Microsoft.Xna.Framework;
using TryToEscape.Core;
using TryToEscape.World;

namespace TryToEscape.Components;

public class VisionComponent : Component
{
    private Maze _maze;
    private Entity _player;
    private int _tileSize;
    private int _radius;
    private float _halfAngleCos;
    private Action _onSpotted;

    public VisionComponent(Maze maze, Entity player, int tileSize, int radius, float halfAngleDegrees, Action onSpotted)
    {
        _maze = maze;
        _player = player;
        _tileSize = tileSize;
        _radius = radius;
        _onSpotted = onSpotted;

        var halfAngleRad = MathHelper.ToRadians(halfAngleDegrees);
        _halfAngleCos = (float)Math.Cos(halfAngleRad);
    }

    public override void Update(GameTime gameTime)
    {
        var patrol = Owner.GetComponent<PatrolMovementComponent>();
        if (patrol == null) return;

        var facing = patrol.Facing;
        if (facing.LengthSquared() == 0) return;

        var playerPos = _player.Position;
        var patrolPos = Owner.Position;

        var patrolTilePos = patrolPos.ToTileCentered(_tileSize);
        var playerTilePos = playerPos.ToTileCentered(_tileSize);

        var toPlayer = playerPos - patrolPos;
        var distance = toPlayer.Length();

        if (distance == 0) 
        {
            _onSpotted();
            return;
        }
        if (distance > _radius * _tileSize) return;
        var dirToPlayer = toPlayer / distance;

        var cos = Vector2.Dot(dirToPlayer, facing);
        if (cos < _halfAngleCos) return; 

        if (!VisibilityCalculator.HasLineOfSight(_maze, playerTilePos.X, playerTilePos.Y, patrolTilePos.X, patrolTilePos.Y))
            return;

        _onSpotted();
    }
}