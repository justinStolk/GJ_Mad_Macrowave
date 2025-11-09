using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class Hero : MonoBehaviour, IDamageable
{
    public ushort Health => health;

    [SerializeField] private ushort health;
    [SerializeField] private float moveSpeed;
    [SerializeField] private ushort attackPower;

    private Rigidbody rb;
    private Vector2 direction;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InputSystem.actions["Move"].performed += Move;
        InputSystem.actions["Move"].canceled += (ctx) => { direction = Vector2.zero; };
        InputSystem.actions["Attack"].performed += Attack;
    }

    void FixedUpdate()
    {
        if (direction.sqrMagnitude > 0.1f)
        {
            rb.MovePosition(transform.position + new Vector3(direction.x, 0, direction.y).normalized * moveSpeed * Time.fixedDeltaTime);
        }
    }
    public void TakeDamage(ushort damage)
    {
        health = (ushort)Mathf.Clamp(health - damage, 0, health);
        if(health == 0)
        {
            OnDeath();
        }
    }


    private void Move(InputAction.CallbackContext context)
    {
        direction = context.ReadValue<Vector2>();
    }

    private void Attack(InputAction.CallbackContext context)
    {
        Debug.Log("Triggered attack!");
    }

    private void OnDeath()
    {
        throw new NotImplementedException();
    }
}
