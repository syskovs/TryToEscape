namespace TryToEscape.Config;

public static class GameConfig
{
    public const int TileSize = 16;
    public const int MazeWidth = 50;
    public const int MazeHeight = 40;
    public const int MinLeafSize = 8;
    public const int MinRoomSize = 4;
    public const int Padding = 1;

    public const int PlayerSpeed = 150;
    public const int FogRadius = 6;
    public const float Zoom = 6f;
    
    public const int PatrolCount = 2;
    public const int WaypointsPerPatrol = 3;
    public const int PatrolVisionRadius = 8;
    public const int PatrolSpeed = 60;
    public const int PatrolVisionAngle = 60;
    public const float GracePeriod = 0f;
    public const int PatrolChaseSpeed = 60;

    public const int KeyCount = 3;
    public const int KeyToExit = 3;
}