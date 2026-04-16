using UnityEngine;
using TMPro;

public class TowerUpgrade : MonoBehaviour
{
    [Header("Range & Money")]
    [SerializeField] private RangeVisualizer rangeVisualizer;
    [SerializeField] private Money money;
    [SerializeField] private MoneyBridge moneyBridge;

    [Header("TowerSelect")]
    [SerializeField] private TowerSelect towerSelect;

    [Header("UI")]
    [SerializeField] private TowerUpgradeUI upgradeUI;

    private TowerAttack selectedTowerAttack;
    private TowerPrice selectedTowerPrice;
    private GameObject selectedTower;

    private MoneyModel _moneyModel;

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
        if (selectedTowerPrice == null) return;

        if (selectedTowerPrice.GetLevel() >= 4)
        {
            upgradeUI?.UpdateUI();
            return;
        }

        int cost = selectedTowerPrice.GetUpgradeCost();
        if (!TryGetCurrentMoney(out int currentMoney))
        {
            Debug.LogError("TowerUpgrade: no money source found (Money or MoneyBridge).");
            return;
        }

        if (currentMoney < cost)
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

        if (!TrySubMoney(cost))
        {
            Debug.LogError("TowerUpgrade: failed to subtract money.");
            return;
        }

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

    private bool TryGetCurrentMoney(out int amount)
    {
        amount = 0;

        if (money != null)
        {
            amount = money.GetCurrMoney();
            return true;
        }

        if (TryResolveMoneyModel())
        {
            amount = _moneyModel.CurrMoney;
            return true;
        }

        return false;
    }

    private bool TrySubMoney(int amount)
    {
        if (money != null)
        {
            money.SubMoney(amount);
            return true;
        }

        if (TryResolveMoneyModel())
        {
            _moneyModel.SubMoney(amount);
            return true;
        }

        return false;
    }

    private bool TryResolveMoneyModel()
    {
        if (_moneyModel != null)
        {
            return true;
        }

        if (moneyBridge == null)
        {
            moneyBridge = FindObjectOfType<MoneyBridge>();
            if (moneyBridge == null)
            {
                return false;
            }
        }

        _moneyModel = moneyBridge.GetModel();
        return _moneyModel != null;
    }
}
