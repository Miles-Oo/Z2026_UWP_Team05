using UnityEngine;
using TMPro;

public class TowerUpgrade : MonoBehaviour
{
    [Header("Range & Money")]
    [SerializeField] private RangeVisualizer rangeVisualizer;
    [SerializeField] private Money money;

    [Header("TowerSelect")]
    [SerializeField] private TowerSelect towerSelect;

    [Header("UI")]
    [SerializeField] private TowerUpgradeUI upgradeUI;

    private TowerAttack selectedTowerAttack;
    private TowerPrice selectedTowerPrice;
    private GameObject selectedTower;

    public void SetSelectedTower(GameObject tower)
    {
        if (tower == null) return;

        selectedTower = tower;
        selectedTowerAttack = tower.GetComponent<TowerAttack>();
        selectedTowerPrice = tower.GetComponent<TowerPrice>();

        if (upgradeUI != null && selectedTowerPrice != null)
            upgradeUI.SetTower(selectedTowerPrice);
    }

    public void UpgradeCurrent()
    {
        if (selectedTower == null) return;

        if (selectedTowerPrice.GetLevel() >= 4)
        {
            upgradeUI?.UpdateUI();
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

        ConstructionSide site = selectedTower.GetComponentInParent<ConstructionSide>();
        int oldLayer = selectedTower.layer;

        Destroy(selectedTower);

        GameObject newTower = Instantiate(nextPrefab, pos, rot);

        if (site != null)
        {
            newTower.transform.SetParent(site.transform);
            newTower.transform.localPosition = Vector3.zero;
            site.SetTower(newTower);
        }

        SetLayerRecursively(newTower, oldLayer);

        selectedTower = newTower;
        selectedTowerAttack = newTower.GetComponent<TowerAttack>();
        selectedTowerPrice = newTower.GetComponent<TowerPrice>();

        money.SubMoney(cost);
        selectedTowerPrice.LevelUp();

        if (towerSelect != null)
            towerSelect.SetSelectedTower(newTower);

        if (rangeVisualizer != null)
            rangeVisualizer.ShowRange(selectedTower.transform.position, selectedTowerAttack.GetRange());

        if (upgradeUI != null)
            upgradeUI.SetTower(selectedTowerPrice);
    }

    private void SetLayerRecursively(GameObject obj, int layer)
    {
        obj.layer = layer;
        foreach (Transform child in obj.transform)
            SetLayerRecursively(child.gameObject, layer);
    }
}