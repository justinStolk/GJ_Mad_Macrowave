using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour
{
    [SerializeField] private TMP_Text fundsText;
    [SerializeField] private RectTransform shopInterface;

    [Header("Tower Buying")]
    [SerializeField] private TowerShopButton buttonTemplate;
    [SerializeField] private RectTransform buttonContainer;
    [SerializeField] private RectTransform descriptorPanel;
    [SerializeField] private TMP_Text descriptionTitle;
    [SerializeField] private TMP_Text descriptionText;

    [Header("Tower Upgrading")]
    [SerializeField] private RectTransform upgradePanelPrefab;


    // This reference is here so it can be dynamically placed based on positioning of the tower
    private RectTransform upgradePanel;

    private void Awake()
    {
        CloseDescription();
    }

    public void UpdateFunds(ushort newFunds)
    {
        fundsText.text = newFunds.ToString();
    }

    public void DisplayUpgrades(Tower towerToUpgrade)
    {
        throw new System.NotImplementedException();
        // Can be implemented when merged with tower upgrades branch
    }

    public void CreateTowerShopInterface(Tower[] allTowers, System.Action<Tower> onButtonActivated)
    {
        for (int i = 0; i < allTowers.Length; i++)
        {
            Tower current = allTowers[i];

            TowerShopButton tsb = Instantiate(buttonTemplate, buttonContainer);
            tsb.AddOnClickEvent(() => onButtonActivated.Invoke(current));
            tsb.SetImageGraphic(current.Icon);
            tsb.SetButtonText($"{current.Name}: {current.Cost}");
            tsb.OnHoverEnter += () => ShowDescription(current);
            tsb.OnHoverExit += CloseDescription;
        }
    }

    private void ShowDescription(Tower towerToDescribe)
    {
        descriptionTitle.text = towerToDescribe.Name;
        descriptionText.text = towerToDescribe.Description;
        descriptorPanel.gameObject.SetActive(true);
    }

    private void CloseDescription()
    {
        descriptorPanel.gameObject.SetActive(false);
    }
}
