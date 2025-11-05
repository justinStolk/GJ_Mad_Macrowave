using UnityEngine;

public interface IDamageable
{
    public ushort Health { get; }
    public void TakeDamage(ushort damage);
}
