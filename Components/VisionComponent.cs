using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
    private float _gracePeriod;
    private float _spotTimer = 0f;
    private Texture2D _pixel;

    public VisionComponent(Maze maze, Entity player, int tileSize, int radius, float halfAngleDegrees, float gracePeriod, Texture2D pixel,Action onSpotted)
    {
        _maze = maze;
        _player = player;
        _tileSize = tileSize;
        _radius = radius;
        _onSpotted = onSpotted;
        _gracePeriod = gracePeriod;
        _pixel = pixel;

        var halfAngleRad = MathHelper.ToRadians(halfAngleDegrees);
        _halfAngleCos = (float)Math.Cos(halfAngleRad);
    }

    public override void Update(GameTime gameTime)
    {
        var dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

        if (IsSpottingPlayer())
        {
            _spotTimer += dt;
            if (_spotTimer >= _gracePeriod)
                _onSpotted();
        }
        else
        {
            _spotTimer = 0f;
        }
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        var patrol = Owner.GetComponent<PatrolMovementComponent>();
        if (patrol == null) return;

        var facing = patrol.Facing;
        if (facing.LengthSquared() == 0) return;

        var patrolTile = Owner.Position.ToTileCentered(_tileSize);

        var alarm = MathHelper.Clamp(_spotTimer / _gracePeriod, 0f, 1f);
        var color = Color.Lerp(Color.Yellow * 0.25f, Color.Red * 0.5f, alarm);

        for (int dx = -_radius; dx <= _radius; dx++)
        for (int dy = -_radius; dy <= _radius; dy++)
        {
            var tile = new Point(patrolTile.X + dx, patrolTile.Y + dy);
            if (!IsTileInView(tile, facing, patrolTile)) continue;

            spriteBatch.Draw(_pixel,
                new Rectangle(tile.X * _tileSize, tile.Y * _tileSize, _tileSize, _tileSize),
                color);
        }
    }

    private bool IsSpottingPlayer()
    {
        var patrol = Owner.GetComponent<PatrolMovementComponent>();
        if (patrol == null) return false;

        var facing = patrol.Facing;
        if (facing.LengthSquared() == 0) return false;

        var patrolTile = Owner.Position.ToTileCentered(_tileSize);
        var playerTile = _player.Position.ToTileCentered(_tileSize);

        return IsTileInView(playerTile, facing, patrolTile);
    }

    private bool IsTileInView(Point tile, Vector2 facing, Point patrolTile)
    {
        var delta = (tile - patrolTile).ToVector2();
        var distance = delta.Length();

        if (distance == 0) return true;
        if (distance > _radius) return false;

        var direction = delta / distance;
        var cos = Vector2.Dot(direction, facing);
        if (cos < _halfAngleCos) return false;

        if (!VisibilityCalculator.HasLineOfSight(_maze, patrolTile.X, patrolTile.Y, tile.X, tile.Y))
            return false;

        return true;
    }
}