using UnityEngine;

public class Barrier : GridObject
{
    public override string Name => name;
    public override string Description => description;
    public override ushort Cost => cost;
    public override Sprite Icon => icon;

    [SerializeField] private new string name;
    [SerializeField] private string description;
    [SerializeField] private ushort cost;
    [SerializeField] private Sprite icon;
}
