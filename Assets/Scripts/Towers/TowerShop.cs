using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class TowerShop : MonoBehaviour
{
    public ushort Funds => funds;

    [SerializeField] private Tower[] availableTowers;
    // This might need to be changed to getting them automatically through resources.
    [SerializeField] private ushort funds = 100;

    // These references should be decoupled in the future
    [SerializeField] private TD_Grid grid;
    [SerializeField] private Spawner spawner;

    [Header("Events")]
    [SerializeField] private UnityEvent<Tower> onTowerShopTowerChanged;
    [SerializeField] private UnityEvent<Tower[], Action<Tower>> onTowerInterfaceCall;
    [SerializeField] private UnityEvent<ushort> onFundsChanged;

    private InputAction placementAction;
    private Tower virtualTower;

    private void Awake()
    {
        placementAction = InputSystem.actions.FindAction("Placement");
        onTowerInterfaceCall?.Invoke(availableTowers, PlanTower);
        Enemy.OnDeathFunds += (ushort funds) => ChangeFunds(funds);
        onFundsChanged?.Invoke(funds);
    }

    private void Update()
    {
        if (virtualTower == null)
        {
            return;
        }
    }

    private void ChangeFunds(int amount)
    {
        funds = (ushort)Mathf.Clamp(funds + amount, 0, ushort.MaxValue);
        onFundsChanged?.Invoke(funds);
    }

    private void PlanTower(Tower towerToBuy)
    {
        if (Funds >= towerToBuy.Cost)
        {
            CreateVirtualTower(towerToBuy);
        }
        else
        {
            Debug.LogWarning("Not enough funds to buy: " + towerToBuy.name);
        }
    }

    private void CreateVirtualTower(Tower template)
    {
        if(virtualTower != null)
        {
            Destroy(virtualTower);
        }
        virtualTower = Instantiate(template);
        virtualTower.DeactivateTower();
        onTowerShopTowerChanged?.Invoke(virtualTower);
        placementAction.started += ValidateAndPlaceTower;
    }

    private void ValidateAndPlaceTower(InputAction.CallbackContext context)
    {
        Vector2Int gridPosition = grid.WorldToGrid(virtualTower.transform.position);
        if (grid.IsPositionOccupied(gridPosition.x, gridPosition.y) || !spawner.EvaluateEndpointAccessability())
        {
            return;
        }
        if(grid.OccupyTowerPoint(virtualTower, gridPosition))
        {
            ChangeFunds(-virtualTower.Cost);
            virtualTower.ActivateTower();
            virtualTower = null;
            onTowerShopTowerChanged?.Invoke(null);
            placementAction.started -= ValidateAndPlaceTower;
            return;
        }
        throw new Exception("Attempting to place a tower where that is not possible! This should not happen!");
    }
}
