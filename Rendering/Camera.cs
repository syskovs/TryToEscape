using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TryToEscape.Rendering;

public class Camera
{
    private Vector2 _position;
    private int _width;
    private int _height;
    private float _zoom;

    public Camera(int width, int height, float zoom)
    {
        _width = width;
        _height = height;
        _zoom = zoom;
    }

    public void Follow(Vector2 target)
    {
        _position = target;
    }

    public Matrix GetTransformMatrix()
    {
        return Matrix.CreateTranslation(-_position.X, -_position.Y, 0) *
           Matrix.CreateScale(_zoom) *
           Matrix.CreateTranslation(_width / 2f, _height / 2f, 0);
    }

    public Rectangle GetVisibleArea() 
    {
        var w = (int)(_width / _zoom);
        var h = (int)(_height / _zoom);

        return new Rectangle(
            (int)(_position.X - w / 2f),
            (int)(_position.Y - h / 2f),
            w, h);
    }

}