using UnityEngine;

public class TowerUpgrade : MonoBehaviour
{

    [SerializeField] private RangeVisualizer rangeVisualizer;
    [SerializeField] private Money money;

    [Header("TowerSelect")]
    [SerializeField] private TowerSelect towerSelect;

    private TowerAttack selectedTowerAttack;
    private TowerPrice selectedTowerPrice;
    private GameObject selectedTower;

    public GameObject GetSelectedTower()
    {
        return selectedTower;
    }

    public void SetSelectedTower(GameObject tower)
    {
        if (tower == null) return;

        selectedTower = tower;
        selectedTowerAttack = tower.GetComponent<TowerAttack>();
        selectedTowerPrice = tower.GetComponent<TowerPrice>();
    }

    public void UpgradeCurrent()
    {
        if (selectedTower == null) return;
        if (selectedTowerPrice == null) return;

        if (selectedTowerPrice.GetLevel() >= 4)
        {
            Debug.Log("MAX LEVEL");
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

        var data = newTower.GetComponent<TowerRuntimeData>();
        if (data == null)
            data = newTower.AddComponent<TowerRuntimeData>();

        data.prefab = nextPrefab;

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

        if (rangeVisualizer != null && selectedTowerAttack != null)
            rangeVisualizer.ShowRange(newTower.transform.position, selectedTowerAttack.GetRange());

        ObserverUpgrade.Instance.OnTowerUpgraded(selectedTower);
    }

    private void SetLayerRecursively(GameObject obj, int layer)
    {
        obj.layer = layer;
        foreach (Transform child in obj.transform)
            SetLayerRecursively(child.gameObject, layer);
    }

    public Money GetMoney()
    {
        return money;
    }

    public void RefreshRange()
    {
        if (selectedTower == null) return;

        var attack = selectedTower.GetComponent<TowerAttack>();
        if (attack == null) return;

        if (rangeVisualizer != null)
        {
            rangeVisualizer.ShowRange(
                selectedTower.transform.position,
                attack.GetRange()
            );
        }
    }

    public GameObject GetTowerFromSite(ConstructionSide site)
    {
        if (site == null) return null;
        return site.GetPlacedTower();
    }
}