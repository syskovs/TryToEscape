using Microsoft.Xna.Framework;
using TryToEscape.Core;
using TryToEscape.World;

namespace TryToEscape.Components;

public class KeyComponent : Component
{
    private Entity _player;
    private Scene _scene;
    private int _tileSize;

    public KeyComponent(Entity player, Scene scene, int tileSize)
    {
        _player = player;
        _scene = scene;
        _tileSize = tileSize;
    }

    public override void Update(GameTime gameTime)
    {
        var playerTileX = (int)((_player.Position.X + _tileSize / 2f) / _tileSize);
        var playerTileY =  (int)((_player.Position.Y + _tileSize / 2f) / _tileSize);
        var keyTileX = (int)((Owner.Position.X + _tileSize / 2f) / _tileSize);
        var keyTileY = (int)((Owner.Position.Y + _tileSize / 2f) / _tileSize);

        if (playerTileX == keyTileX && playerTileY == keyTileY)
        {
            _player.GetComponent<InventoryComponent>().AddKey();
            _scene.RemoveEntity(Owner);
        }
    }
}