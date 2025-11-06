using System;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    public ushort Health => health;

    [SerializeField] private ushort health;
    [SerializeField] private float moveSpeed;
    [SerializeField] private ushort endPointDamage = 1;
    [SerializeField] private ushort heroDamage = 1;

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
