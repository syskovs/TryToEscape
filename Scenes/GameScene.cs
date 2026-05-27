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

    public GameScene(ContentManager contentManager)
    {
        var background = contentManager.Load<Texture2D>("assets/background");
        var sprite = contentManager.Load<Texture2D>("assets/sprite");

        // var backgroundEntity = new Entity();
        // backgroundEntity.AddComponent(new SpriteComponent(background));
        // AddEntity(backgroundEntity);

        var maze = new MazeGenerator().Generate(50, 30, 8, 4, 1);
        
        var spriteEntity = new Entity();
        spriteEntity.AddComponent(new SpriteComponent(sprite));
        spriteEntity.AddComponent(new InputComponent(100));
        spriteEntity.Position = new Vector2(5 * 16, 5 * 16);
        spriteEntity.AddComponent(new ColliderComponent(maze, 16, 16));
        AddEntity(spriteEntity);

        var floorTexture = contentManager.Load<Texture2D>("assets/tiles/floor");
        var wallTexture  = contentManager.Load<Texture2D>("assets/tiles/wall");
        _mazeRenderer = new MazeRenderer(maze, floorTexture, wallTexture, 16);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        
        _mazeRenderer.Draw(spriteBatch);
        base.Draw(spriteBatch);
    }
}