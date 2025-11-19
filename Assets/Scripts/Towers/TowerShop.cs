using UnityEngine;
using UnityEngine.UI;

public class TowerShop : MonoBehaviour
{
    [SerializeField] private Tower[] availableTowers;
    // This might need to be changed to getting them automatically through resources.
    [SerializeField] private GameObject shopInterface;
    [SerializeField] private ushort funds = 100;

    private Tower virtualTower;

    private void Update()
    {
        if(virtualTower == null)
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

    private void BuyTower(Tower towerToBuy)
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
    }
}
