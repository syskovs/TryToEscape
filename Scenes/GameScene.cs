using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TryToEscape.Components;
using TryToEscape.Core;

namespace TryToEscape.Scenes;

public class GameScene : Scene
{
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

    }
}