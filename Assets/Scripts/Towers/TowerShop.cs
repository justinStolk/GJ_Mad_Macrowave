using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class TowerShop : MonoBehaviour
{
    [SerializeField] private Tower[] availableTowers;
    // This might need to be changed to getting them automatically through resources.
    [SerializeField] private GameObject shopInterface;
    [SerializeField] private RectTransform buttonContainer;
    [SerializeField] private Button buttonTemplate;
    [SerializeField] private ushort funds = 100;

    // These references should be decoupled in the future
    [SerializeField] private TD_Grid grid;
    [SerializeField] private Spawner spawner;

    [SerializeField] private UnityEvent<Tower> onTowerShopTowerChanged;

    private InputAction placementAction;
    private Tower virtualTower;

    private void Awake()
    {
        placementAction = InputSystem.actions.FindAction("Placement");
        CreateInterface(availableTowers, buttonTemplate, buttonContainer);
    }



    private void Update()
    {
        if (virtualTower == null)
        {
            return;
        }

    }

    public void OnTowerShopOpened()
    {
        shopInterface.SetActive(true);
    }

    public void OnTowerShopClosed()
    {
        shopInterface.SetActive(false);
    }

    private void PlanTower(Tower towerToBuy)
    {
        if (funds >= towerToBuy.Cost)
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
        virtualTower = Instantiate(template);
        onTowerShopTowerChanged?.Invoke(virtualTower);
        placementAction.started += ValidateAndPlaceTower;
    }
    private void CreateInterface(Tower[] allTowers, Button buttonTemplate, RectTransform buttonContainer)
    {
        for (int i = 0; i < allTowers.Length; i++)
        {
            Tower current = allTowers[i];
            Button towerButton = Instantiate(buttonTemplate, buttonContainer);
            towerButton.GetComponentInChildren<TMPro.TMP_Text>().text = $"{current.Name}: {current.Cost}";
            towerButton.onClick.AddListener(() => PlanTower(current));
        }
    }

    private void ValidateAndPlaceTower(InputAction.CallbackContext context)
    {
        Vector2Int gridPosition = grid.WorldToGrid(virtualTower.transform.position);
        if (grid.IsPositionOccupied(gridPosition.x, gridPosition.y) || !spawner.EvaluateEndpointAccessability())
        {
            return;
        }
        if(grid.OccupyTowerPoint(virtualTower, gridPosition.x, gridPosition.y))
        {
            virtualTower.ActivateTower();
            virtualTower = null;
            onTowerShopTowerChanged?.Invoke(null);
            placementAction.started -= ValidateAndPlaceTower;
            return;
        }
        throw new System.Exception("Attempting to place a tower where that is not possible! This should not happen!");
    }
}
