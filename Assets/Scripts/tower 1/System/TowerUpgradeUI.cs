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

    public void UpdateUI()
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

        Texture nextTexture = currentTower.GetNextLevelTexture();

        if (currentTower.GetLevel() >= 4 || nextTexture == null)
        {
            upgradeImage.enabled = false;
            return;
        }

        upgradeImage.enabled = true;
        upgradeImage.texture = nextTexture;
    }
}