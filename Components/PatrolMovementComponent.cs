using System;
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
    private List<Point> _currentPath;
    private int _pathIndex;
    private State _state;

    private Entity _chaseTarget;
    private float _chaseSpeed;

    private Action _onCatch;
    
    public enum State
    {
        Patrolling,
        Chasing
    }

    public bool IsChasing()
    {
        return _state == State.Chasing;
    }

    public PatrolMovementComponent(Maze maze, IReadOnlyList<Point> waypoints, int tileSize, float speed, float chaseSpeed, Action onCatch)
    {
        _maze = maze;
        _waypoints = waypoints;
        _tileSize = tileSize;
        _speed = speed;
        _chaseSpeed = chaseSpeed;
        _onCatch = onCatch;
    }


    public void StartChasing(Entity target)
    {
        _chaseTarget = target;
        if (_state != State.Chasing)
        {
            _state = State.Chasing;

            Point fromTile;
            if (_currentPath != null && _pathIndex < _currentPath.Count)
                fromTile = _currentPath[_pathIndex];
            else
                fromTile = Owner.Position.ToTile(_tileSize);

            var toTile = _chaseTarget.Position.ToTile(_tileSize);
            _currentPath = Pathfinder.FindPath(_maze, fromTile, toTile);
            _pathIndex = 0;
        }
    }

    public void StopChasing()
    {
        if (_state == State.Chasing)
        {
            _state = State.Patrolling;
            _chaseTarget = null;
             RecalculatePatrolPath();
            _pathIndex = 0;
        }
    }

    public override void Update(GameTime gameTime)
    {
        if (_waypoints.Count < 2) return;
        if (!_initialized) { InitFirstPath(); _initialized = true; }
        if (_currentPath == null || _currentPath.Count == 0) return;

        var dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
        var target = _currentPath[_pathIndex].ToPixel(_tileSize);
        var speed = _state == State.Chasing ? _chaseSpeed : _speed;
        MoveToward(target, dt, speed);

        if (Owner.Position == target)
            AdvanceToNextTile();

        CheckCatch();
    }

    private void AdvanceToNextTile()
    {
        _pathIndex++;
        if (_pathIndex >= _currentPath.Count)
        {
            if (_state == State.Chasing) RecalculateChasePath();
            else { AdvanceWaypoint();  RecalculatePatrolPath(); }
            _pathIndex = _currentPath.Count > 1 ? 1 : 0;
        }
    }

    private void CheckCatch()
    {
        if (_state != State.Chasing || _chaseTarget == null) return;
        if (Vector2.Distance(Owner.Position, _chaseTarget.Position) < _tileSize)
            _onCatch?.Invoke();
    }

    private void RecalculateChasePath()
    {
        if (_chaseTarget == null) return;
        var fromTile = Owner.Position.ToTile(_tileSize);
        var toTile = _chaseTarget.Position.ToTile(_tileSize);
        _currentPath = Pathfinder.FindPath(_maze, fromTile, toTile);
    }

    private void MoveToward(Vector2 targetPixel, float dt, float speed)
    {
        var delta = targetPixel - Owner.Position;
        var distance = delta.Length();
        var step = speed * dt;

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

    private void InitFirstPath()
    {
        _waypointIndex = 0;
        _waypointDirection = 1;
         RecalculatePatrolPath();
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

    private void  RecalculatePatrolPath()
    {
        var fromTile = Owner.Position.ToTile(_tileSize);
        var toTile = _waypoints[_waypointIndex];
        _currentPath = Pathfinder.FindPath(_maze, fromTile, toTile);
    }
}