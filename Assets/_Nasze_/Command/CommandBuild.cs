using UnityEngine;

public class CommandBuild : ICommand
{
    private GameObject prefab;
    private ConstructionSide site;
    private Money money;
    private int cost;

    public CommandBuild(GameObject prefab, ConstructionSide site, Money money, int cost)
    {
        this.prefab = prefab;
        this.site = site;
        this.money = money;
        this.cost = cost;
    }

    public void Execute()
    {
        money.SubMoney(cost);

        GameObject tower = Object.Instantiate(prefab, site.transform.position, Quaternion.identity);
        tower.transform.SetParent(site.transform);
        tower.transform.localPosition = Vector3.zero;

        var data = tower.GetComponent<TowerRuntimeData>() ?? tower.AddComponent<TowerRuntimeData>();
        data.prefab = prefab;

        site.SetTower(tower);
    }

    public void Undo()
    {
        GameObject tower = site.GetPlacedTower();
        if (tower == null) return;

        money.AddMoney(cost);

        site.SetTower(null);
        Object.Destroy(tower);
    }

    public void Redo()
    {
        Execute();
    }
}