using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TowerTargetStrategyUI : MonoBehaviour
{
    private TowerAttack currentTower;
    private SlowTowerController currentSlowTower;

    private bool isNearest = true;
    private bool isKnockback = false;


    [SerializeField] private Button toggleButton;
    [SerializeField] private TMP_Text buttonText;

    public void SetTower(TowerAttack tower)
    {
        currentTower = tower;
        currentSlowTower = null;
        if (currentTower.GetCurrentStrategy() is NearestEnemyStrategy)
            isNearest = true;
        else
            isNearest = false;
        ApplyStrategy();
    }

    public void SetSlowTower(SlowTowerController tower)
    {
        currentSlowTower = tower;
        currentTower = null;
        isKnockback = tower.IsKnockbackMode();

        ApplySlowTowerMode();
    }

    public void ToggleStrategy()
    {

        if (currentTower != null)
        {
            isNearest = !isNearest;
            ApplyStrategy();
            return;
        }

        if (currentSlowTower != null)
        {
            isKnockback = !isKnockback;

            ApplySlowTowerMode();
            return;
        }
    }

    private void ApplyStrategy()
    {

        if (currentTower == null)
            return;

        if (isNearest)
        {
            currentTower.SetTargetStrategy(new NearestEnemyStrategy());

            if (buttonText != null)
                buttonText.text = "Nearest";
        }
        else
        {
            currentTower.SetTargetStrategy(new WeakestEnemyStrategy());

            if (buttonText != null)
                buttonText.text = "Weakest";
        }
        RefreshUI();
    }

    private void ApplySlowTowerMode()
    {
        if (currentSlowTower == null)
            return;

        currentSlowTower.SetMode(isKnockback);

        if (isKnockback)
        {
            if (buttonText != null)
                buttonText.text = "Knockback";
        }
        else
        {
            if (buttonText != null)
                buttonText.text = "Slow";
        }
        RefreshUI();
    }
    private void RefreshUI()
    {
        if (buttonText == null) return;

        if (currentTower != null)
            buttonText.text = isNearest ? "Nearest" : "Weakest";

        if (currentSlowTower != null)
            buttonText.text = isKnockback ? "Knockback" : "Slow";
    }
}