using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TryToEscape.Entities;

namespace TryToEscape.Core;

public class GameContext
{
    public ContentManager Content { get; }
    public GraphicsDevice Graphics { get; }
    public SceneManager Scenes { get; }
    public EntityFactory Entities { get; }
    public Texture2D Pixel { get; }

    public GameContext(ContentManager content, GraphicsDevice graphics, SceneManager scenes)
    {
        Content = content;
        Graphics = graphics;
        Scenes = scenes;
        Entities = new EntityFactory(content);

        Pixel = new Texture2D(graphics, 1, 1);
        Pixel.SetData(new[] { Color.White });
    }
}