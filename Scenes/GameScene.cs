using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TryToEscape.Components;
using TryToEscape.Core;
using TryToEscape.Rendering;
using TryToEscape.World;

namespace TryToEscape.Scenes;

public class GameScene : Scene
{
    private MazeRenderer _mazeRenderer;
    private Camera _camera;
    private Entity _spriteEntity;
    

    public GameScene(ContentManager contentManager, GraphicsDevice graphicsDevice)
    {
        _camera = new Camera(graphicsDevice.Viewport.Width, graphicsDevice.Viewport.Height);
        var sprite = contentManager.Load<Texture2D>("assets/sprite");
        var maze = new MazeGenerator().Generate(50, 30, 8, 4, 1);
        
        _spriteEntity = new Entity();
        _spriteEntity.AddComponent(new SpriteComponent(sprite));
        _spriteEntity.AddComponent(new InputComponent(100));
        _spriteEntity.Position = new Vector2(5 * 16, 5 * 16);
        _spriteEntity.AddComponent(new ColliderComponent(maze, 16, 16));
        AddEntity(_spriteEntity);

        var floorTexture = contentManager.Load<Texture2D>("assets/tiles/floor");
        var wallTexture  = contentManager.Load<Texture2D>("assets/tiles/wall");
        _mazeRenderer = new MazeRenderer(maze, floorTexture, wallTexture, 16);
    }

    public override Matrix GetCameraMatrix()
    {
        return _camera.GetTransformMatrix();
    }

    public override void Update(GameTime gameTime)
    {
        _camera.Follow(_spriteEntity.Position);
        base.Update(gameTime);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {   
        _mazeRenderer.Draw(spriteBatch);
        base.Draw(spriteBatch);
    }
}