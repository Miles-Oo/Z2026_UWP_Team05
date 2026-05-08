using UnityEngine;

public class CommandSell : ICommand, ICommandWithHistory
{
    private TowerSelect towerSelect;
    private GameObject prefab;
    private ConstructionSide site;
    private Money money;
    private int refund;
    private int level;

    private GameObject soldTower;
    private TowerUpgrade towerUpgrade;

    public bool WasSkipped { get; private set; }

    public CommandSell(GameObject tower, ConstructionSide site, int level, int refund, Money money, TowerUpgrade towerUpgrade, TowerSelect towerSelect)
    {
        this.site = site;
        this.money = money;
        this.refund = refund;
        this.level = level;
        this.towerUpgrade = towerUpgrade;
        this.towerSelect = towerSelect;

        if (tower != null)
        {
            soldTower = tower;

            var data = tower.GetComponent<TowerRuntimeData>();
            if (data != null)
                prefab = data.prefab;
        }
    }

    public void Execute()
    {
        if (soldTower == null)
            soldTower = site.GetPlacedTower();

        if (soldTower == null) return;

        var data = soldTower.GetComponent<TowerRuntimeData>();
        int sellLevel = data != null ? data.level : 1;
        TowerPrice price = soldTower.GetComponent<TowerPrice>();
        int basePrice = price != null ? price.GetPrice() : 0;

        int finalRefund = Mathf.RoundToInt(basePrice * sellLevel * 0.6f); 

        money.AddMoney(finalRefund);

        site.SetTower(null);
        Object.Destroy(soldTower);

        if (towerUpgrade != null)
        {
            towerSelect.ForceClearSelection();
        }
    }

    public void Undo()
    {
        if (site == null || prefab == null) return;

        GameObject restored = Object.Instantiate(prefab, site.transform.position, Quaternion.identity);
        restored.transform.SetParent(site.transform);
        restored.transform.localPosition = Vector3.zero;

        var data = restored.GetComponent<TowerRuntimeData>();
        if (data == null)
            data = restored.AddComponent<TowerRuntimeData>();

        data.prefab = prefab;

        var price = restored.GetComponent<TowerPrice>();
        if (price != null)
            price.SetLevel(level);

        site.SetTower(restored);

        money.SubMoney(refund);

        if (towerUpgrade != null)
        {
            towerSelect.ForceClearSelection();
        }
    }

    public void Redo()
    {
        Execute();
    }
}