using System.Collections.Generic;
using UnityEngine;

public class SingleTargetTower : Tower
{
    protected override bool _hasTarget => target != null;

    [SerializeField] private Transform projectileSpawnpoint;
    [SerializeField] private Projectile projectilePrefab;

    private Enemy target;

    protected override void AttackTarget()
    {
        base.AttackTarget();
        Projectile projectile = Instantiate(projectilePrefab, projectileSpawnpoint.position, Quaternion.identity);
        projectile.Launch(target.transform.position + Vector3.up * 0.5f - projectile.transform.position, 25f, power);
    }

    protected override void FindTarget()
    {
        if(target != null && IsTargetStillInRange())
        {
            return;
        }

        target = null;
        Collider[] hits = Physics.OverlapSphere(transform.position, range.y, ~excludedLayers);
        if (hits.Length == 0) return;
        // There's no need to do any clearing or further calculations if nothing is in range.

        foreach (Collider col in hits)
        {
            if (!Physics.Raycast(transform.position, col.transform.position, out _, range.x, ~excludedLayers) && col.TryGetComponent(out Enemy enemy))
            {
                target = enemy;
                return;
            }
        }
    }

    private bool IsTargetStillInRange()
    {
        bool targetIsTooClose = Physics.Raycast(transform.position, target.transform.position, out _, range.x, ~excludedLayers);
        bool targetIsNotTooFar = Physics.Raycast(transform.position, target.transform.position, out _, range.y, ~excludedLayers);
        return !targetIsTooClose && targetIsNotTooFar;
    }
}
