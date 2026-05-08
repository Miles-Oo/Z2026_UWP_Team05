using UnityEngine;
using TMPro;

public class SlowTowerHoverInfo : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject infoPanel;
    [SerializeField] private TextMeshProUGUI textField;
    [SerializeField] private TextMeshProUGUI nameField;

    private const int MAX_LEVEL = 4;

    private string GetUpgradeText(int level, int cost)
    {
        return level >= MAX_LEVEL ? "MAX LEVEL" : cost.ToString();
    }

    public void Show(SlowTowerStats stats, int level = 1)
    {
        if (stats == null || infoPanel == null) return;

        infoPanel.SetActive(true);

        nameField.text = "Kinetic Suppressor";

        textField.text =
            $"Level: {level}\n" +
            $"Damage: {stats.damage}\n" +
            $"Range: {stats.range}\n" +
            $"Attack speed: {stats.attackInterval}s\n\n" +
            $"Slow: {stats.slowPercent * 100f}%\n" +
            $"Slow duration: {stats.slowDuration}s\n\n" +
            $"Upgrade cost: {GetUpgradeText(level, stats.upgradeCost)}";
    }

    // 🟢 BASIC TOWER
    public void ShowBasicTower(int damage, float range, float attackSpeed, int level, int upgradeCost)
    {
        if (infoPanel == null) return;

        infoPanel.SetActive(true);

        nameField.text = "Bastion Tower";

        textField.text =
            $"Level: {level}\n" +
            $"Damage: {damage}\n" +
            $"Range: {range}\n" +
            $"Attack speed: {attackSpeed}s\n\n" +
            $"Upgrade cost: {GetUpgradeText(level, upgradeCost)}";
    }

    public void Hide()
    {
        if (infoPanel == null) return;
        infoPanel.SetActive(false);
    }
}