using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TryToEscape.Core;

namespace TryToEscape.Components;

public class AnimatedSpriteComponent : Component
{
    private Texture2D _sheet;
    private int _frameWidth;
    private int _frameHeight;
    private int _frameCount;
    private float _frameDuration;
    private bool _pauseWhenStill;

    private float _elapsedTime;
    private int _currentFrame;
    private Vector2 _prevPosition;
    private bool _facingLeft;

    public AnimatedSpriteComponent(
        Texture2D sheet, 
        int frameWidth, int frameHeight, 
        int frameCount, 
        float frameDuration,
        bool pauseWhenStill = false)
    {
        _sheet = sheet;
        _frameWidth = frameWidth;
        _frameHeight = frameHeight;
        _frameCount = frameCount;
        _frameDuration = frameDuration;
        _pauseWhenStill = pauseWhenStill;
    }

    public override void Update(GameTime gameTime)
    {
        var delta = Owner.Position - _prevPosition;
        if (delta.X > 0) _facingLeft = false;
        else if (delta.X < 0) _facingLeft = true;
        bool moved = Owner.Position != _prevPosition;
        _prevPosition = Owner.Position;

        if (_pauseWhenStill && !moved)
        {
            _currentFrame = 0;
            _elapsedTime = 0;
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
        var sourceRect = new Rectangle(
            _currentFrame * _frameWidth, 0,
            _frameWidth, _frameHeight);
        var effects = _facingLeft 
            ? SpriteEffects.FlipHorizontally 
            : SpriteEffects.None;

        spriteBatch.Draw(
            _sheet, 
            Owner.Position, 
            sourceRect, 
            Color.White,
            rotation: 0f, 
            origin: Vector2.Zero, 
            scale: 1f, 
            effects: effects, 
            layerDepth: 0f);
    }
}