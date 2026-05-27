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

        var backgroundEntity = new Entity();
        backgroundEntity.AddComponent(new SpriteComponent(background));
        AddEntity(backgroundEntity);

        var spriteEntity = new Entity();
        spriteEntity.AddComponent(new SpriteComponent(sprite));
        spriteEntity.AddComponent(new InputComponent(100));
        AddEntity(spriteEntity);

        var floorTexture = contentManager.Load<Texture2D>("assets/tiles/floor");
        var wallTexture  = contentManager.Load<Texture2D>("assets/tiles/wall");
        var maze = new MazeGenerator().Generate(40, 25, 8, 3, 1);
        _mazeRenderer = new MazeRenderer(maze, floorTexture, wallTexture, 16);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        base.Draw(spriteBatch);
        _mazeRenderer.Draw(spriteBatch);
    }
}