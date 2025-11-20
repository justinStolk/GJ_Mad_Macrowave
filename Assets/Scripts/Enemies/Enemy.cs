using System;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour, IDamageable
{
    public ushort Health => health;

    [SerializeField] private ushort health;
    [SerializeField] private float moveSpeed;
    [SerializeField] private ushort endPointDamage = 1;
    [SerializeField] private ushort heroDamage = 1;

    private NavMeshAgent agent;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if(!agent.pathPending && agent.remainingDistance < 0.25f)
        {
            Debug.Log($"{name} reached the end point! This deals {endPointDamage} points of end point damage!");
            Destroy(gameObject);
        }
    }

    public void SetTarget(Vector3 position)
    {
        agent.SetDestination(position);
    }

    public void TakeDamage(ushort damage)
    {
        health = (ushort)Mathf.Clamp(health - damage, 0, health);
        if(health == 0)
        {
            OnDeath();
        }
    }

    private void OnDeath()
    {
        Destroy(gameObject);
        // Also give the player money or something.
    }
}
