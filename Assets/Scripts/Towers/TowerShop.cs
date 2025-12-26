using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class TowerShop : MonoBehaviour
{
    public ushort Funds => funds;

    [SerializeField] private GridObject[] gridObjects;
    // This might need to be changed to getting them automatically through resources.
    [SerializeField] private ushort funds = 100;

    // These references should be decoupled in the future
    [SerializeField] private TD_Grid grid;
    [SerializeField] private TowerRangeRenderer tRenderer;
    [SerializeField] private SpawnerV2 spawner;

    //[SerializeField] private Material validMaterial;
    //[SerializeField] private Material invalidMaterial;

    [Header("Events")]
    [SerializeField] private UnityEvent<GridObject> onTowerShopTowerChanged;
    [SerializeField] private UnityEvent<GridObject[], Action<GridObject>> onTowerInterfaceCall;
    [SerializeField] private UnityEvent<ushort> onFundsChanged;

    private InputAction placementAction;
    private GridObject virtualGridObject;
    private List<Material> towerMaterials;

    private void Awake()
    {
        placementAction = InputSystem.actions.FindAction("Placement");
        onTowerInterfaceCall?.Invoke(gridObjects, PlanTower);
        Enemy.OnDeathFunds += (ushort funds) => ChangeFunds(funds);
        onFundsChanged?.Invoke(funds);
    }

    private void ChangeFunds(int amount)
    {
        funds = (ushort)Mathf.Clamp(funds + amount, 0, ushort.MaxValue);
        onFundsChanged?.Invoke(funds);
    }

    private void PlanTower(GridObject objectToBuy)
    {
        if (Funds >= objectToBuy.Cost)
        {
            CreateVirtualGridObject(objectToBuy);
        }
        else
        {
            Debug.LogWarning("Not enough funds to buy: " + objectToBuy.name);
        }
    }

    private void CreateVirtualGridObject(GridObject template)
    {
        if(virtualGridObject != null)
        {
            Destroy(virtualGridObject);
        }
        virtualGridObject = Instantiate(template);
        if(virtualGridObject is Tower tower)
        {
            tower.DeactivateTower();
        }
        virtualGridObject.GetComponent<NavMeshObstacle>().enabled = false;
        //Material[] vtMaterials = virtualTower.GetComponentInChildren<MeshRenderer>().materials;
        //for (int i = 0; i < vtMaterials.Length; i++)
        //{
        //    towerMaterials.Add(vtMaterials[i]);
        //    vtMaterials[i] = validMaterial;
        //}

        onTowerShopTowerChanged?.Invoke(virtualGridObject);
        placementAction.started += ValidateTower;
    }

    private void ValidateTower(InputAction.CallbackContext context)
    {
        Vector2Int gridPosition = grid.WorldToGrid(virtualGridObject.transform.position);
        if (grid.IsPositionOccupied(gridPosition.x, gridPosition.y))
        {
            return;
        }
        NavMeshObstacle towerNMObstacle = virtualGridObject.GetComponent<NavMeshObstacle>();

        towerNMObstacle.enabled = true;
        StartCoroutine(spawner.EvaluateEndPointAccessability(towerNMObstacle, PlaceTowerIfValid));
    }

    private void PlaceTowerIfValid(bool positionIsValid)
    {
        if (!positionIsValid)
        {
            InputSystem.actions.FindAction("Positioning").Enable();
            return;
        }
        Vector2Int gridPosition = grid.WorldToGrid(virtualGridObject.transform.position);

        if (grid.OccupyTowerPoint(virtualGridObject, gridPosition))
        {
            ChangeFunds(-virtualGridObject.Cost);
            if(virtualGridObject is Tower tower)
            {
                tower.ActivateTower();
            }
            virtualGridObject = null;
            onTowerShopTowerChanged?.Invoke(null);
            placementAction.started -= ValidateTower;
            tRenderer.StopRender();
            InputSystem.actions.FindAction("Positioning").Enable();

            return;
        }
        throw new Exception("Attempting to place a tower where that is not possible! This should not happen!");
    }
}
