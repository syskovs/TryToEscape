using System.Collections.Generic;
using Microsoft.Xna.Framework;
using TryToEscape.Core;
using TryToEscape.World;

namespace TryToEscape.Components;

public class PatrolMovementComponent : Component
{

    public Vector2 Facing { get; private set; }
    private Maze _maze;
    private IReadOnlyList<Point> _waypoints;
    private int _tileSize;
    private float _speed;

    private bool _initialized;
    private int _waypointIndex;
    private int _waypointDirection = 1;
    private IReadOnlyList<Point> _currentPath;
    private int _pathIndex;

    public PatrolMovementComponent(Maze maze, IReadOnlyList<Point> waypoints, int tileSize, float speed)
    {
        _maze = maze;
        _waypoints = waypoints;
        _tileSize = tileSize;
        _speed = speed;
    }

    public override void Update(GameTime gameTime)
    {
        if (_waypoints.Count < 2) return;
        if (!_initialized) { InitFirstPath(); _initialized = true; }

        var dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

        if (_currentPath == null || _pathIndex >= _currentPath.Count) {
            AdvanceWaypoint();
            RecalculatePath();
            if (_currentPath.Count == 0) return;
            _pathIndex = 0;
        }

        var target = TileToPixelCenter(_currentPath[_pathIndex]);
        MoveToward(target, dt);
        if (Owner.Position == target) _pathIndex++;
    }

    private void InitFirstPath()
    {
        _waypointIndex = 0;
        _waypointDirection = 1;
        RecalculatePath();
        _pathIndex = 0;
    }

    private void AdvanceWaypoint()
    {
        _waypointIndex += _waypointDirection;

        if (_waypointIndex >= _waypoints.Count) {
            _waypointDirection = -1;
            _waypointIndex = _waypoints.Count - 2;
        }

        if (_waypointIndex < 0) {
            _waypointDirection = 1;
            _waypointIndex = 1;
        }
    }

    private void RecalculatePath()
    {
        var fromTile = PixelToTile(Owner.Position);
        var toTile = _waypoints[_waypointIndex];
        _currentPath = Pathfinder.FindPath(_maze, fromTile, toTile);
    }

    private void MoveToward(Vector2 targetPixel, float dt)
    {
        var delta = targetPixel - Owner.Position;
        var distance = delta.Length();
        var step = _speed * dt;

        if (distance <= step)
        {
            Owner.Position = targetPixel;
        }
        else
        {
            var direction = delta / distance;
            Owner.Position += direction * step;
            Facing = direction;
        }
    }

    private Vector2 TileToPixelCenter(Point tile) 
    {
        return new Vector2(tile.X * _tileSize, tile.Y * _tileSize);
    }

    private Point PixelToTile(Vector2 pixel) 
    {
        return new Point(
            (int)(pixel.X / _tileSize),
            (int)(pixel.Y / _tileSize)
        );
    }
}