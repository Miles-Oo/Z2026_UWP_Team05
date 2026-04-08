using UnityEngine;
using TMPro;

public class TowerUpgrade : MonoBehaviour
{
    [SerializeField] private RangeVisualizer rangeVisualizer;
    [SerializeField] private Money money;
    [SerializeField] private TextMeshProUGUI upgradeCostText;

    [Header("UI Images")]
    [SerializeField] private Transform towersUI;

    [Header("Reference to TowerSelect")]
    [SerializeField] private TowerSelect towerSelect;

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

        UpdateTowerImages();
        UpgradeSelectedTower();
    }

    /////////////////
    public void UpgradeCurrent()
    {
        if (selectedTower == null) return;
        UpgradeSelectedTower();
        if (upgradeUI != null)
            upgradeUI.SetTower(selectedTowerPrice); // odśwież obrazek
    }

    /////////
    public void SetCurrentTower(GameObject tower)
    {
        selectedTower = tower;
        selectedTowerAttack = tower.GetComponent<TowerAttack>();
        selectedTowerPrice = tower.GetComponent<TowerPrice>();
    }

    public void UpdateTowerImagesForSelected(TowerPrice towerPrice)
    {
        selectedTowerPrice = towerPrice;
        UpdateTowerImages();
    }

    private void UpgradeSelectedTower()
    {
        if (selectedTowerAttack == null || selectedTowerPrice == null) return;

        if (selectedTowerPrice.GetLevel() >= 4)
        {
            if (upgradeCostText != null)
                upgradeCostText.text = "MAX LEVEL";
            UpdateTowerImages();
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
        int oldLevel = selectedTowerPrice.GetLevel();

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
            towerSelect.SetSelectedTower(newTower); /////// selectedTower

        if (rangeVisualizer != null)
            rangeVisualizer.ShowRange(selectedTower.transform.position, selectedTowerAttack.GetRange());

        var upgradeTextComp = upgradeCostText.GetComponent<UpgradeCostText>();
        if (upgradeTextComp != null)
        {
            upgradeTextComp.SetTower(selectedTowerPrice);
        }

        UpdateTowerImages();
        Debug.Log($"Tower upgraded to level {selectedTowerPrice.GetLevel()}");

//////////////////////////
        if (upgradeUI != null)
        {
            upgradeUI.SetTower(selectedTowerPrice);
        }
    }

    private void UpdateTowerImages()
    {
        if (towersUI == null || selectedTowerPrice == null) return;

        int level = selectedTowerPrice.GetLevel();

        for (int i = 0; i < towersUI.childCount; i++)
            towersUI.GetChild(i).gameObject.SetActive(false);

        if (level >= 4) return;

        int index = level - 1;
        if (index >= 0 && index < towersUI.childCount)
            towersUI.GetChild(index).gameObject.SetActive(true);
    }

    private void SetLayerRecursively(GameObject obj, int layer)
    {
        obj.layer = layer;
        foreach (Transform child in obj.transform)
            SetLayerRecursively(child.gameObject, layer);
    }
}