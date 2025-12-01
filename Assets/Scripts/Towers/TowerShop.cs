using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
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
    [SerializeField] private TowerRangeRenderer tRenderer;
    [SerializeField] private Spawner spawner;

    //[SerializeField] private Material validMaterial;
    //[SerializeField] private Material invalidMaterial;

    [Header("Events")]
    [SerializeField] private UnityEvent<Tower> onTowerShopTowerChanged;
    [SerializeField] private UnityEvent<Tower[], Action<Tower>> onTowerInterfaceCall;
    [SerializeField] private UnityEvent<ushort> onFundsChanged;

    private InputAction placementAction;
    private Tower virtualTower;
    private List<Material> towerMaterials;

    private void Awake()
    {
        placementAction = InputSystem.actions.FindAction("Placement");
        onTowerInterfaceCall?.Invoke(availableTowers, PlanTower);
        Enemy.OnDeathFunds += (ushort funds) => ChangeFunds(funds);
        onFundsChanged?.Invoke(funds);
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
        virtualTower.GetComponent<NavMeshObstacle>().enabled = false;
        //Material[] vtMaterials = virtualTower.GetComponentInChildren<MeshRenderer>().materials;
        //for (int i = 0; i < vtMaterials.Length; i++)
        //{
        //    towerMaterials.Add(vtMaterials[i]);
        //    vtMaterials[i] = validMaterial;
        //}

        onTowerShopTowerChanged?.Invoke(virtualTower);
        placementAction.started += ValidateAndPlaceTower;
    }

    private void ValidateAndPlaceTower(InputAction.CallbackContext context)
    {
        Vector2Int gridPosition = grid.WorldToGrid(virtualTower.transform.position);
        if (grid.IsPositionOccupied(gridPosition.x, gridPosition.y))
        {
            return;
        }
        NavMeshObstacle towerNMObstacle = virtualTower.GetComponent<NavMeshObstacle>();

        towerNMObstacle.enabled = true;
        if (!spawner.EvaluateEndpointAccessability())
        {
            towerNMObstacle.enabled = false;
            return;
        }

        if (grid.OccupyTowerPoint(virtualTower, gridPosition))
        {
            ChangeFunds(-virtualTower.Cost);
            virtualTower.ActivateTower();
            virtualTower = null;
            onTowerShopTowerChanged?.Invoke(null);
            placementAction.started -= ValidateAndPlaceTower;
            tRenderer.StopRender();
            return;
        }
        throw new Exception("Attempting to place a tower where that is not possible! This should not happen!");
    }
}
