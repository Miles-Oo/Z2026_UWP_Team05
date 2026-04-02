using UnityEngine;
using TMPro;

public class TowerUpgrade : MonoBehaviour
{
    [SerializeField] private RangeVisualizer rangeVisualizer;
    [SerializeField] private Money money;
    [SerializeField] private TextMeshProUGUI upgradeCostText;

    [Header("UI Images")]
    [SerializeField] private Transform towersUI; // parent LV2, LV3, LV4

    [Header("Reference to TowerSelect")]
    [SerializeField] private TowerSelect towerSelect;

    private TowerAttack selectedTowerAttack;
    private TowerPrice selectedTowerPrice;
    private GameObject selectedTower;

    // Ustawienie wybranej wieży i od razu jej ulepszenie
    public void SetSelectedTower(GameObject tower)
    {
        if (tower == null) return;

        selectedTower = tower;
        selectedTowerAttack = tower.GetComponent<TowerAttack>();
        selectedTowerPrice = tower.GetComponent<TowerPrice>();

        UpdateTowerImages(); // pokaż kolejny poziom w UI

        UpgradeSelectedTower();
    }

    // Aktualizacja UI dla wybranej wieży bez upgrade
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
            Debug.Log("MAX LEVEL");
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

        Destroy(selectedTower);

        GameObject newTower = Instantiate(nextPrefab, pos, rot);

        if (site != null)
        {
            newTower.transform.SetParent(site.transform);
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

        if (upgradeCostText != null)
        {
            if (selectedTowerPrice.GetLevel() >= 4)
                upgradeCostText.text = "MAX LEVEL";
            else
                upgradeCostText.text = $"Upgrade: {selectedTowerPrice.GetUpgradeCost()}";
        }

        UpdateTowerImages(); // pokaż kolejny poziom po upgrade

        Debug.Log($"Tower upgraded to level {selectedTowerPrice.GetLevel()}");
    }

    // 🔥 Aktualizacja obrazków wież w UI
    private void UpdateTowerImages()
    {
        if (towersUI == null || selectedTowerPrice == null) return;

        int level = selectedTowerPrice.GetLevel();

        // ukryj wszystkie dzieci
        for (int i = 0; i < towersUI.childCount; i++)
            towersUI.GetChild(i).gameObject.SetActive(false);

        // MAX LEVEL → nic nie pokazuj
        if (level >= 4) return;

        // pokaż tylko następny poziom: LV1→LV2, LV2→LV3, LV3→LV4
        int index = level - 1;

        if (index >= 0 && index < towersUI.childCount)
        {
            towersUI.GetChild(index).gameObject.SetActive(true);
            Debug.Log($"Tower level: {level}, showing child index: {index} ({towersUI.GetChild(index).name})");
        }
        else
        {
            Debug.LogWarning($"Tower level {level}, index {index} is out of range! towersUI.childCount = {towersUI.childCount}");
        }
    }

    // Rekurencyjna zmiana warstwy wieży i wszystkich dzieci
    private void SetLayerRecursively(GameObject obj, int layer)
    {
        obj.layer = layer;

        foreach (Transform child in obj.transform)
        {
            SetLayerRecursively(child.gameObject, layer);
        }
    }
}