using UnityEngine;

[CreateAssetMenu(fileName = "TowerUpgrade", menuName = "Scriptable Objects/TowerUpgrade")]
public class TowerUpgrade : ScriptableObject
{
    public string UpgradeName => upgradeName;
    public string UpgradeDescription => upgradeDescription;
    public ushort UpgradeCost => upgradeCost;
    public ushort PowerIncrease => powerIncrease;
    public ushort RangeIncrease => rangeIncrease;
    public TowerUpgrade[] NextUnlockedUpgrades => nextUnlockedUpgrades;

    [SerializeField] private string upgradeName;
    [SerializeField, TextArea(2,3)] private string upgradeDescription;
    [SerializeField] private ushort upgradeCost;
    [SerializeField] private ushort powerIncrease;
    [SerializeField] private ushort rangeIncrease;
    [SerializeField] private TowerUpgrade[] nextUnlockedUpgrades;
}
