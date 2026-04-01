using UnityEngine;
using TMPro;

public class TowerUpgrade : MonoBehaviour
{
    [SerializeField] private RangeVisualizer rangeVisualizer;
    [SerializeField] private Money money;
    [SerializeField] private TextMeshProUGUI upgradeCostText;
    [Header("Reference to TowerSelect")]
    [SerializeField] private TowerSelect towerSelect;

    private TowerAttack selectedTowerAttack;
    private TowerPrice selectedTowerPrice;
    private GameObject selectedTower;

    public void SetSelectedTower(GameObject tower)
    {
        if (tower == null) return;

        selectedTower = tower;
        selectedTowerAttack = tower.GetComponent<TowerAttack>();
        selectedTowerPrice = tower.GetComponent<TowerPrice>();

        UpgradeSelectedTower();
    }

    private void UpgradeSelectedTower()
    {
        if (selectedTowerAttack == null || selectedTowerPrice == null) return;

        if (selectedTowerPrice.GetLevel() >= 4)
        {
            Debug.Log("MAX LEVEL");
            if (upgradeCostText != null)
                upgradeCostText.text = "MAX LEVEL";
            return;
        }

        int cost = selectedTowerPrice.GetUpgradeCost();
        if (money.GetCurrMoney() < cost)
        {
            Debug.Log("Not enough money!");
            return;
        }

        GameObject nextPrefab = selectedTowerPrice.GetNextLevelPrefab();
        if (nextPrefab == null)
        {
            Debug.LogError("Next level prefab not assigned!");
            return;
        }

        Vector3 pos = selectedTower.transform.position;
        Quaternion rot = selectedTower.transform.rotation;

        // Pobieramy ConstructionSide PRZED zniszczeniem starej wieży
        ConstructionSide site = selectedTower.GetComponentInParent<ConstructionSide>();

        Destroy(selectedTower);

        GameObject newTower = Instantiate(nextPrefab, pos, rot);

        // Ustawiamy nową wieżę w ConstructionSide
        if (site != null)
        {
            site.SetTower(newTower);
        }

        // Aktualizacja referencji
        selectedTower = newTower;
        selectedTowerAttack = newTower.GetComponent<TowerAttack>();
        selectedTowerPrice = newTower.GetComponent<TowerPrice>();

        money.SubMoney(cost);
        selectedTowerPrice.LevelUp();

        // Aktualizacja TowerSelect
        if (towerSelect != null)
            towerSelect.SetSelectedTower(newTower);

        // Wyświetlenie nowego zasięgu
        if (rangeVisualizer != null)
            rangeVisualizer.ShowRange(selectedTower.transform.position, selectedTowerAttack.GetRange());

        // Aktualizacja UI
        if (upgradeCostText != null)
        {
            if (selectedTowerPrice.GetLevel() >= 4)
                upgradeCostText.text = "MAX LEVEL";
            else
                upgradeCostText.text = $"Upgrade: {selectedTowerPrice.GetUpgradeCost()}";
        }

        Debug.Log($"Tower upgraded to level {selectedTowerPrice.GetLevel()} with range {selectedTowerAttack.GetRange()}");
    }
}