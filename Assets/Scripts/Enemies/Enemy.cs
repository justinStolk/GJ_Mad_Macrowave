using System;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour, IDamageable
{
    public static Action<Enemy> OnEnemySpawn;
    public ushort Health => health;

    [SerializeField] private ushort health;
    [SerializeField] private float moveSpeed;
    [SerializeField] private ushort endPointDamage = 1;
    [SerializeField] private ushort heroDamage = 1;

    private NavMeshAgent agent;
    private bool initialized;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = moveSpeed;
        OnEnemySpawn?.Invoke(this);
    }

    private void Update()
    {
        if(initialized && !agent.pathPending && agent.remainingDistance < 0.25f)
        {
            Debug.Log($"{name} reached the end point! This deals {endPointDamage} points of end point damage!");
            Destroy(gameObject);
        }
    }

    public void SetTarget(Vector3 position)
    {
        agent.SetDestination(position);
        initialized = true;
    }

    public void TakeDamage(ushort damage)
    {
        health = (ushort)Mathf.Clamp(health - damage, 0, health);
        if(health == 0)
        {
            OnDeath();
        }
    }

    protected virtual void OnDeath()
    {
        Destroy(gameObject);
        // Also give the player money or something.
    }
}
