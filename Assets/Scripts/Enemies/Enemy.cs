using System;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour, IDamageable
{
    public static Action<Enemy> OnEnemySpawn;
    public static Action<ushort> OnEndpointReached;
    public static Action<ushort> OnDeathFunds;
    public ushort Health => health;

    [SerializeField] private ushort health;
    [SerializeField] private float moveSpeed;
    [SerializeField] private ushort endPointDamage = 1;
    [SerializeField] private ushort onKillFundsReceived;
    [SerializeField] private AudioSource deathSoundSource;

    private NavMeshAgent agent;
    private bool initialized;
    private Vector3 destination;
    private static float distanceCheckInterval = 0.2f;
    private float distanceCheckTimer;

    private void Awake()
    {
        MenuHandler.OnGamePaused += () => agent.isStopped = true;
        MenuHandler.OnGameResumed += () => agent.isStopped = false;
        agent = GetComponent<NavMeshAgent>();
        agent.speed = moveSpeed;
        OnEnemySpawn?.Invoke(this);
        distanceCheckTimer = Time.time + distanceCheckInterval;
    }

    private void Update()
    {
        if (!initialized) return;

        if(distanceCheckTimer <= Time.time)
        {
            distanceCheckTimer = Time.time + distanceCheckInterval;
            float distance = Vector3.Distance(destination, transform.position);
            if(distance <= agent.stoppingDistance + 0.5f)
            {
                OnEndpointReached?.Invoke(endPointDamage);
                Destroy(gameObject);
            }
        }
    }

    public void SetTarget(Vector3 position)
    {
        agent.SetDestination(position);
        destination = position;
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

    // This is a very hack-like way to do this, which should be resolved later
    public void ToggleStall(bool activated)
    {
        agent.isStopped = activated;
    }

    protected virtual void OnDeath()
    {
        OnDeathFunds?.Invoke(onKillFundsReceived);
        if(deathSoundSource != null)
        {
            deathSoundSource.Play();
        }
        Destroy(gameObject);
    }
}
