using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class TowerPlacer : MonoBehaviour
{
    [SerializeField] private UnityEvent<GridObject, Vector3> onPositionChanged;
    [SerializeField] private LayerMask groundLayer;

    private GridObject referencedGridObject;

    private void Awake()
    {
        InputSystem.actions.FindAction("Positioning").performed += UpdatePosition;
        InputSystem.actions.FindActionMap("Placement").Enable();
    }

    public void SetTowerReference(GridObject gridObject)
    {
        referencedGridObject = gridObject;
    }

    private void UpdatePosition(InputAction.CallbackContext context)
    {
        if (referencedGridObject == null) return;

        Vector2 mousePosition = context.ReadValue<Vector2>();
        Ray mouseRay = Camera.main.ScreenPointToRay(mousePosition);
        // Rework this later

        if (Physics.Raycast(mouseRay, out RaycastHit hit, 99, groundLayer))
        {
            Vector3 worldPosition = hit.point;
            onPositionChanged?.Invoke(referencedGridObject, worldPosition);
        }
    }
}
