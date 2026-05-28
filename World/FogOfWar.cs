namespace TryToEscape.World;

public class FogOfWar
{
    private State[,] _visibility;
    private Maze _maze;

    public FogOfWar(Maze maze)
    {
        _visibility = new State[maze.Width, maze.Height];
        _maze = maze;
    }

    public enum State
    {
        Hidden,
        Visible,
        Explored
    }

    public State GetState(int x, int y)
    {
        if (x < 0 || x >= _maze.Width || y < 0 || y >= _maze.Height)
            return State.Hidden;

        return _visibility[x, y];
    }

    public void UpdateFrom(int originX, int originY, int radius)
    {
        for (var x = 0; x < _maze.Width; x++)
            for (var y = 0; y < _maze.Height; y++)
                if (_visibility[x, y] == State.Visible) 
                    _visibility[x, y] = State.Explored;
        
        foreach (var (x, y) in VisibilityCalculator.Compute(_maze, originX, originY, radius))
            _visibility[x, y] = State.Visible;
    }

    public int Width => _maze.Width;
    public int Height => _maze.Height;
}