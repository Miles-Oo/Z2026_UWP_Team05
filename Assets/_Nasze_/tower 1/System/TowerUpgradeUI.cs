using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TowerUpgradeUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI upgradeCostText;
    [SerializeField] private RawImage upgradeImage;

    private TowerPrice currentTower;

    public void SetTower(TowerPrice towerPrice)
    {
        currentTower = towerPrice;
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (currentTower == null) return;

        UpdateText();
        UpdateImage();
    }

    private void UpdateText()
    {
        if (upgradeCostText == null) return;

        if (currentTower.GetLevel() >= 4 || currentTower.GetNextLevelPrefab() == null)
            upgradeCostText.text = "MAX LEVEL";
        else
            upgradeCostText.text = $"Upgrade: {currentTower.GetUpgradeCost()}";
    }

    private void UpdateImage()
    {
        if (upgradeImage == null) return;

        Texture tex = currentTower.GetNextLevelTexture();

        if (tex == null)
        {
            upgradeImage.enabled = false; // ukryj
        }
        else
        {
            upgradeImage.enabled = true;
            upgradeImage.texture = tex;
        }
    }
}