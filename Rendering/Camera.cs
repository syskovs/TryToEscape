using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TryToEscape.Rendering;

public class Camera
{
    private Vector2 _position;
    private int _width;
    private int _height;

    public Camera(int width, int height)
    {
        _width = width;
        _height = height;
    }

    public void Follow(Vector2 target)
    {
        _position = target;
    }

    public Matrix GetTransformMatrix()
    {
        return Matrix.CreateTranslation(-_position.X + _width / 2, -_position.Y + _height / 2, 0);
    }

}