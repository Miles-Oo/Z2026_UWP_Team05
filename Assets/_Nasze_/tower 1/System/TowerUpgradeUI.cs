using UnityEngine;
using TMPro;

public class TowerUpgradeUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI upgradeCostText;
    [SerializeField] private Transform towersUI; // parent LV2, LV3, LV4

    private TowerPrice currentTower;

    public void SetTower(TowerPrice towerPrice)
    {
        currentTower = towerPrice;
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (currentTower == null) return;

        int level = currentTower.GetLevel();

        UpdateText(level);
        UpdateImages(level);
    }

    private void UpdateText(int level)
    {
        if (upgradeCostText == null) return;

        if (level >= 4)
            upgradeCostText.text = "MAX LEVEL";
        else
            upgradeCostText.text = $"Upgrade: {currentTower.GetUpgradeCost()}";
    }

    private void UpdateImages(int level)
    {
        if (towersUI == null) return;

        // ukryj wszystkie
        for (int i = 0; i < towersUI.childCount; i++)
            towersUI.GetChild(i).gameObject.SetActive(false);

        // MAX LEVEL → nic
        if (level >= 4) return;

        // pokaż tylko kolejny poziom: LV1→LV2, LV2→LV3, LV3→LV4
        int index = level - 1;

        if (index >= 0 && index < towersUI.childCount)
            towersUI.GetChild(index).gameObject.SetActive(true);
    }
}