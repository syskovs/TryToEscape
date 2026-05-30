using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using TryToEscape.Core;

namespace TryToEscape.Components;

public class PauseTriggerComponent : Component
{
    private Action _onPress;
    private bool _wasPressed;

    public PauseTriggerComponent(Action onPress)
    {
        _onPress = onPress;
    }

    public override void Update(GameTime gameTime)
    {
        var isPressed = Keyboard.GetState().IsKeyDown(Keys.Escape);

        if (!isPressed && _wasPressed)
            _onPress();

        _wasPressed = isPressed;
    }
}