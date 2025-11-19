using System.Collections.Generic;
using UnityEngine;

public class MultiTargetTower : Tower
{
    protected override bool _hasTarget => targets.Count > 0;

    [SerializeField, Min(2)] private ushort maximumSimultaneousTargets = 2;
    [SerializeField] private Transform projectileSpawnpoint;
    [SerializeField] private Projectile projectilePrefab;

    private List<Enemy> targets = new();

    protected override void AttackTarget()
    {
        base.AttackTarget();
        foreach(Enemy enemy in targets)
        {
            Projectile projectile = Instantiate(projectilePrefab, projectileSpawnpoint.position, Quaternion.identity);
            projectile.Launch(enemy.transform.position + Vector3.up * 0.5f - projectile.transform.position, 25f, power);
        }
    }

    protected override void FindTarget()
    {
        if (AreTargetsStillInRange())
        {
            return;
        }

        targets.Clear();

        Collider[] hits = Physics.OverlapSphere(transform.position, range.y, ~excludedLayers);
        if (hits.Length == 0) return;
        // There's no need to do any clearing or further calculations if nothing is in range.

        foreach (Collider col in hits)
        {
            if (!Physics.Raycast(transform.position, col.transform.position, out _, range.x, ~excludedLayers) && col.TryGetComponent(out Enemy enemy))
            {
                targets.Add(enemy);
                if(targets.Count == maximumSimultaneousTargets)
                {
                    return;
                }
            }
        }
    }

    private bool AreTargetsStillInRange()
    {
        foreach(Enemy enemy in targets)
        {
            Ray ray = new(transform.position, enemy.transform.position);
            if(!Physics.Raycast(ray, out _, range.y, ~excludedLayers) || Physics.Raycast(ray, out _, range.x, ~excludedLayers))
            {
                return false;
            }
        }
        return true;
    }
}
