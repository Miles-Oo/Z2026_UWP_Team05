using UnityEngine;

public class CommandSell : ICommand
{
    private GameObject towerInstance;
    private GameObject towerPrefab;

    private ConstructionSide site;
    private int level;
    private int refund;
    private Money money;

    public CommandSell(GameObject towerInstance, ConstructionSide site, int level, int refund, Money money)
    {
        this.towerInstance = towerInstance;
        this.site = site;
        this.level = level;
        this.refund = refund;
        this.money = money;
    }

    public void Execute()
    {
        if (towerInstance == null || site == null)
            return;

        GameObject tower = site.GetPlacedTower();

        if (tower == null)
            return;

        TowerRuntimeData data = tower.GetComponent<TowerRuntimeData>();

        if (data == null)
            return;

        towerPrefab = data.prefab;

        if (towerPrefab == null)
            return;

        money.AddMoney(refund);

        site.SetTower(null);

        Object.Destroy(tower);
    }

    public void Undo()
    {

        if (site == null || towerPrefab == null)
            return;

        GameObject restored = Object.Instantiate(towerPrefab);

        restored.transform.SetParent(site.transform);
        restored.transform.localPosition = Vector3.zero;

        site.SetTower(restored);

        TowerRuntimeData data = restored.GetComponent<TowerRuntimeData>();
        if (data == null)
            data = restored.AddComponent<TowerRuntimeData>();

        data.prefab = towerPrefab;

        TowerPrice price = restored.GetComponent<TowerPrice>();
        if (price != null)
            price.SetLevel(level);

        money.SubMoney(refund);
    }
}