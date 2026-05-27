using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using TryToEscape.Core;

namespace TryToEscape.Components;

public class InputComponent : Component
{
    private float _speed;
    public Vector2 PreviousPosition { get; private set; }

    public InputComponent(float speed)
    {
        _speed = speed;
    }

    public override void Update(GameTime gameTime)
    {
        PreviousPosition = Owner.Position;

        var keyboard = Keyboard.GetState();
        var dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

        if (keyboard.IsKeyDown(Keys.W))
            Owner.Position.Y -= _speed * dt;

        if (keyboard.IsKeyDown(Keys.A))
            Owner.Position.X -= _speed * dt;

        if (keyboard.IsKeyDown(Keys.S))
            Owner.Position.Y += _speed * dt;

        if (keyboard.IsKeyDown(Keys.D))
            Owner.Position.X += _speed * dt;
    }

}