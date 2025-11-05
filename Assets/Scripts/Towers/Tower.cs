using System;
using System.Collections.Generic;
using UnityEngine;

public enum TowerAttackType { Single, Area, Multi }

public class Tower : MonoBehaviour
{
    [SerializeField, Tooltip("How much it costs to create this tower")] private ushort cost;
    [SerializeField, Tooltip("The amount of damage dealt per hit")] private ushort power;
    [SerializeField, Tooltip("The amount of time between hits")] private float attackInterval;
    [SerializeField, Tooltip("The maximum range of the tower")] private float range;
    // Consideration: towers with a minimum range?
    [SerializeField, Tooltip("What kind of attack this tower has")] private TowerAttackType attackType;
    // Conditional formatting should be added for area and multi target towers (or this has to be a inherited class?)
    [SerializeField, Tooltip("Which layers are excluded?")] private LayerMask excludedLayers;

    private List<Enemy> enemiesInRange;
    private float attackTimer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemiesInRange = new();
    }

    // Update is called once per frame
    void Update()
    {
        attackTimer -= Time.deltaTime;

        if (attackTimer > 0) return;
        // We cannot attack yet and thus, don't have to do anything.

        Collider[] hits = Physics.OverlapSphere(transform.position, range, ~excludedLayers);
        if (hits.Length == 0) return;
        // There's no need to do any clearing or further calculations if nothing is in range.

        enemiesInRange.Clear();
        foreach(Collider col in hits)
        {
            if(col.TryGetComponent(out Enemy enemy))
            {
                enemiesInRange.Add(enemy);
                Debug.Log("Enemy with name of: " + enemy.name + " is in range of " + name);
            }
        }
        if(enemiesInRange.Count > 0)
        {
            DetermineTarget(enemiesInRange);
        }
    }

    private void DetermineTarget(List<Enemy> enemiesInRange)
    {
        switch (attackType)
        {
            case TowerAttackType.Single:
                IDamageable damageable = enemiesInRange[0] as IDamageable;
                damageable.TakeDamage(power);
                break;

            case TowerAttackType.Area:
                throw new NotImplementedException();
                break;

            case TowerAttackType.Multi:
                foreach(Enemy enemy in enemiesInRange)
                {
                    IDamageable target = enemy as IDamageable;
                    target.TakeDamage(power);
                }
                break;

        }
        attackTimer = attackInterval;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.aliceBlue;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
