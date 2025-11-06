using System;
using System.Collections.Generic;
using UnityEngine;


public abstract class Tower : MonoBehaviour
{
    public ushort Cost => cost;
    public ushort Power => power;
    public float AttackInterval => attackInterval;
    public Vector2 Range => range;

    protected abstract bool _hasTarget { get; }

    [SerializeField, Tooltip("How much it costs to create this tower")] private ushort cost;
    [SerializeField, Tooltip("The amount of damage dealt per hit")] protected ushort power;
    [SerializeField, Tooltip("The amount of time between hits")] protected float attackInterval;
    [SerializeField, Min(0), Tooltip("The range of the tower (min & max)")] protected Vector2 range = Vector2.up;
    [SerializeField, Tooltip("Which layers are excluded?")] protected LayerMask excludedLayers;

    private float attackTimer;


    // Update is called once per frame
    void Update()
    {
        attackTimer -= Time.deltaTime;

        if (attackTimer > 0) return;
        // We cannot attack yet and thus, don't have to do anything.

        FindTarget();

        if (!_hasTarget) return;

        AttackTarget();
    }

    protected virtual void AttackTarget()
    {
        attackTimer = attackInterval;
    }

    protected abstract void FindTarget();

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.darkRed;
        Gizmos.DrawWireSphere(transform.position, range.x);
        Gizmos.color = Color.aliceBlue;
        Gizmos.DrawWireSphere(transform.position, range.y);
    }
}
