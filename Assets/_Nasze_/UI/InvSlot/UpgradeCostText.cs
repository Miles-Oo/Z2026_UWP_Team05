using TMPro;
using UnityEngine;

public class UpgradeCostText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    private TowerPrice currentTower;

    public void SetTower(TowerPrice towerPrice)
    {
        currentTower = towerPrice;
        UpdateText();
    }

    public void UpdateText()
    {
        if (currentTower == null || text == null) return;

        // MAX LEVEL jeśli nie ma kolejnego prefab albo osiągnięto 4 poziom
        if (currentTower.GetNextLevelPrefab() == null || currentTower.GetLevel() >= 4)
        {
            text.text = "MAX LEVEL";
        }
        else
        {
            text.text = $"Upgrade: {currentTower.GetUpgradeCost()}";
        }
    }
}