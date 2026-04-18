using UnityEngine;

public class CommandBuild : ICommand
{
    private GameObject prefab;
    private ConstructionSide site;
    private Money money;
    private int cost;

    private GameObject builtTower;

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

        builtTower = Object.Instantiate(prefab, site.transform.position, Quaternion.identity);
        builtTower.transform.SetParent(site.transform);
        builtTower.transform.localPosition = Vector3.zero;

        TowerRuntimeData data = builtTower.GetComponent<TowerRuntimeData>();

        if (data == null)
            data = builtTower.AddComponent<TowerRuntimeData>();

        data.prefab = prefab;

        site.SetTower(builtTower);
    }

    public void Undo()
    {
        if (builtTower == null) return;

        money.AddMoney(cost);

        site.SetTower(null);
        Object.Destroy(builtTower);

        builtTower = null;
    }
}