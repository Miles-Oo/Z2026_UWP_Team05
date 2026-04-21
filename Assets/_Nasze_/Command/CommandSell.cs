using UnityEngine;

public class CommandSell : ICommand
{
    private GameObject prefab;
    private ConstructionSide site;
    private Money money;
    private int refund;
    private int level;

    private GameObject soldTower;

    public CommandSell(GameObject tower, ConstructionSide site, int level, int refund, Money money)
    {
        this.site = site;
        this.money = money;
        this.refund = refund;
        this.level = level;

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

        money.AddMoney(refund);

        site.SetTower(null);
        Object.Destroy(soldTower);
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
    }

    public void Redo()
    {
        Execute();
    }
}