using UnityEngine;

public class CommandBuild : ICommand, ICommandWithHistory
{
    private GameObject prefab;
    private ConstructionSide site;
    private Money money;
    private int cost;

    private TowerSelect towerSelect;
    private TowerUpgrade towerUpgrade;

    public bool WasSkipped { get; private set; }

    public CommandBuild(GameObject prefab, ConstructionSide site, Money money, int cost,
                        TowerSelect towerSelect,
                        TowerUpgrade towerUpgrade)
    {
        this.prefab = prefab;
        this.site = site;
        this.money = money;
        this.cost = cost;

        this.towerSelect = towerSelect;
        this.towerUpgrade = towerUpgrade;
    }

    public void Execute()
    {
        money.SubMoney(cost);

        ITowerPrototype prototype = prefab.GetComponent<ITowerPrototype>();
        GameObject tower = prototype.Clone(site.transform.position);
        // GameObject tower = Object.Instantiate(prefab, site.transform.position, Quaternion.identity);
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

        if (towerSelect != null)
            towerSelect.ForceClearSelection();

        if (towerUpgrade != null)
            towerUpgrade.ClearSelection();
    }

    public void Redo()
    {
        Execute();
    }
}