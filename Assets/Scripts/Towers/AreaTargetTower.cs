using System.Collections.Generic;
using UnityEngine;

public class AreaTargetTower : Tower
{
    protected override bool _hasTarget => enemiesInRange.Count > 0;

    private List<Enemy> enemiesInRange = new();

    protected override void AttackTarget()
    {
        base.AttackTarget();
        foreach(IDamageable target in enemiesInRange)
        {
            target.TakeDamage(power);
        }
    }

    protected override void FindTarget()
    {
        enemiesInRange.Clear();
        Collider[] hits = Physics.OverlapSphere(transform.position, range.y, ~excludedLayers);
        if (hits.Length == 0) return;
        // There's no need to do any clearing or further calculations if nothing is in range.

        foreach (Collider col in hits)
        {
            if (!Physics.Raycast(transform.position, col.transform.position, out _, range.x, ~excludedLayers) && col.TryGetComponent(out Enemy enemy))
            {
                enemiesInRange.Add(enemy);
                Debug.Log("Enemy with name of: " + enemy.name + " is in range of " + name);
            }
        }
    }
}
