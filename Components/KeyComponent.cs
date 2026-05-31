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
        var playerTilePos = _player.Position.ToTileCentered(_tileSize);
        var keyTilePos = Owner.Position.ToTileCentered(_tileSize);

        if (playerTilePos == keyTilePos)
        {
            _player.GetComponent<InventoryComponent>().AddKey();
            _scene.RemoveEntity(Owner);
        }
    }
}