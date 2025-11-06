using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Hero : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private ushort attackPower;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InputSystem.actions["Move"].performed += Move;
        InputSystem.actions["Move"].canceled += Move;
    }

    private void Move(InputAction.CallbackContext context)
    {
        Vector2 direction = context.ReadValue<Vector2>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
