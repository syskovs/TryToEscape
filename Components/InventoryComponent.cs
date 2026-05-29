using TryToEscape.Core;

namespace TryToEscape.Components;

public class InventoryComponent : Component
{
    public int KeyCount { get; private set; }
    public bool HasKey => KeyCount > 0;

    public void AddKey()
    {
        KeyCount++;
    }
}