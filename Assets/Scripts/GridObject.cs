using UnityEngine;

public abstract class GridObject : MonoBehaviour
{
    public abstract string Name { get; }
    public abstract string Description { get; }
    public abstract ushort Cost { get; }
    public abstract Sprite Icon { get; }
}
