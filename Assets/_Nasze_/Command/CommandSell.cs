using UnityEngine;

public class CommandSell : ICommand
{
    private GameObject towerPrefab;
    private ConstructionSide site;
    private int level;
    private int refund;
    private Money money;

    private GameObject destroyedTower;

    public CommandSell(GameObject tower, ConstructionSide site, int level, int refund, Money money)
    {
        this.site = site;
        this.level = level;
        this.refund = refund;
        this.money = money;

        if (tower != null)
        {
            var data = tower.GetComponent<TowerRuntimeData>();
            if (data != null)
                towerPrefab = data.prefab;

            destroyedTower = tower;
        }
    }

    public void Execute()
    {
        GameObject tower = site.GetPlacedTower();
        if (tower == null) return;

        var data = tower.GetComponent<TowerRuntimeData>();
        if (data != null)
            towerPrefab = data.prefab;

        money.AddMoney(refund);

        site.SetTower(null);
        Object.Destroy(tower);
    }

    public void Undo()
    {
        if (site == null || towerPrefab == null) return;

        GameObject restored = Object.Instantiate(towerPrefab);

        restored.transform.SetParent(site.transform);
        restored.transform.localPosition = Vector3.zero;

        var data = restored.GetComponent<TowerRuntimeData>();
        if (data == null)
            data = restored.AddComponent<TowerRuntimeData>();

        data.prefab = towerPrefab;

        var price = restored.GetComponent<TowerPrice>();
        if (price != null)
            price.SetLevel(level);

        site.SetTower(restored);
        money.SubMoney(refund);
    }
}