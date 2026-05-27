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
        var texture = contentManager.Load<Texture2D>("assets/background");

        var entity = new Entity();
        entity.AddComponent(new SpriteComponent(texture));
        AddEntity(entity);
    }
}