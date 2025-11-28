using System.Collections.Generic;
using UnityEngine;

public class SingleTargetTower : Tower
{
    protected override bool _hasTarget => target != null;

    [SerializeField] private Transform projectileSpawnpoint;
    [SerializeField] private Projectile projectilePrefab;
    [SerializeField] private Transform aimElement;

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
            AimAtTarget();
            return;
        }

        target = null;
        Collider[] hits = Physics.OverlapSphere(transform.position, range.y, ~excludedLayers);
        if (hits.Length == 0) return;
        // There's no need to do any clearing or further calculations if nothing is in range.

        foreach (Collider col in hits)
        {
            if(col.TryGetComponent(out Enemy enemy))
            {
                float distance = Vector2.Distance(new Vector2(transform.position.x, transform.position.z), new Vector2(enemy.transform.position.x, enemy.transform.position.z));
                if(distance > range.x && distance < range.y)
                {
                    target = enemy;
                    AimAtTarget();
                    return;
                }
            }
        }
    }

    private bool IsTargetStillInRange()
    {
        float distance = Vector2.Distance(new Vector2(transform.position.x, transform.position.z), new Vector2(target.transform.position.x, target.transform.position.z));
        return distance > range.x && distance < range.y;
    }

    private void AimAtTarget()
    {
        if (aimElement != null)
        {
            Vector3 enm = target.transform.position;
            Vector3 aim = aimElement.position;
            // Misaim could be caused by rotation value jumping to negative. Consider using modulo operator or adding a full 360 on top.
            Vector3 direction = new(enm.x - aim.x, 0, enm.z - aim.z);
            float angle = (Mathf.Atan2(direction.z, direction.x) * Mathf.Rad2Deg) - 90f;
            if(angle < 0)
            {
                angle += 360f;
            }
            aimElement.rotation = Quaternion.Euler(0, angle, 0);
        }
    }
}
