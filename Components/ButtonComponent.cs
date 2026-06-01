using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using TryToEscape.Core;

namespace TryToEscape.Components;

public class ButtonComponent : Component
{
    private int _width;
    private int _height;
    private Action _onClick;
    private bool _wasPressed;
    private float _hoverScale = 1.1f;
    private float _normalScale = 1f;

    public ButtonComponent(int width, int height, Action onClick)
    {
        _width = width;
        _height = height;
        _onClick = onClick;
    }

    public override void Update(GameTime gameTime)
    {
        var mouse = Mouse.GetState();
        var rect = new Rectangle(
            (int)Owner.Position.X, (int)Owner.Position.Y, _width, _height);

        bool isHovered = rect.Contains(mouse.X, mouse.Y);

        var sprite = Owner.GetComponent<SpriteComponent>();
        if (sprite != null)
            sprite.Scale = isHovered ? _hoverScale : _normalScale;

        var isPressed = mouse.LeftButton == ButtonState.Pressed;
        if (!isPressed && _wasPressed && isHovered)
            _onClick();
        _wasPressed = isPressed;
    }
}