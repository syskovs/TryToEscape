using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TryToEscape.Core;

namespace TryToEscape.Components;

public class PatrolIndicatorComponent : Component
{
    private Texture2D _sheet;
    private int _frameWidth;
    private int _frameHeight;
    private int _frameCount;
    private float _frameDuration;
    private Vector2 _offset;

    private float _elapsedTime;
    private int _currentFrame;

    public PatrolIndicatorComponent(
        Texture2D sheet,
        int frameWidth, int frameHeight,
        int frameCount, float frameDuration,
        Vector2 offset)
    {
        _sheet = sheet;
        _frameWidth = frameWidth;
        _frameHeight = frameHeight;
        _frameCount = frameCount;
        _frameDuration = frameDuration;
        _offset = offset;
    }

    public override void Update(GameTime gameTime)
    {
        var patrol = Owner.GetComponent<PatrolMovementComponent>();
        if (patrol == null || !patrol.IsChasing())
        {
            _currentFrame = 0;
            _elapsedTime = 0f;
            return;
        }

        _elapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
        if (_elapsedTime >= _frameDuration)
        {
            _elapsedTime -= _frameDuration;
            _currentFrame = (_currentFrame + 1) % _frameCount;
        }
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        var patrol = Owner.GetComponent<PatrolMovementComponent>();
        if (patrol == null || !patrol.IsChasing()) return;

        var sourceRect = new Rectangle(
            _currentFrame * _frameWidth, 0,
            _frameWidth, _frameHeight);
        spriteBatch.Draw(_sheet, Owner.Position + _offset, sourceRect, Color.White);
    }
}