using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TowerUpgradeUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI upgradeCostText;  // Tekst kosztu ulepszenia
    [SerializeField] private RawImage upgradeImage;            // Obrazek do wyświetlenia w przycisku

    private TowerPrice currentTower;

    // Ustawienie wieży, której upgrade ma być wyświetlany
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

    // Aktualizacja tekstu przycisku
    private void UpdateText()
    {
        if (upgradeCostText == null) return;

        if (currentTower.GetLevel() >= 4 || currentTower.GetNextLevelPrefab() == null)
            upgradeCostText.text = "MAX LEVEL";
        else
            upgradeCostText.text = $"Upgrade: {currentTower.GetUpgradeCost()}";
    }

    // Aktualizacja obrazka przycisku
    private void UpdateImage()
    {
        if (upgradeImage == null) return;

        Texture nextTexture = currentTower.GetNextLevelTexture();

        if (currentTower.GetLevel() >= 4 || nextTexture == null)
        {
            // Brak kolejnego poziomu wieży → wyłączamy obrazek
            upgradeImage.enabled = false;
            return;
        }

        // Ustawienie obrazka RawImage w przycisku
        upgradeImage.enabled = true;
        upgradeImage.texture = nextTexture;
    }
}