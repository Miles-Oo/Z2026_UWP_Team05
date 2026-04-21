using UnityEngine;

public class CommandUpgrade : ICommand
{
    private TowerUpgrade upgradeSystem;
    private Money money;

    private ConstructionSide site;

    private GameObject oldPrefab;
    private GameObject newPrefab;

    private int oldLevel;
    private int newLevel;
    private int cost;

    public CommandUpgrade(TowerUpgrade upgradeSystem)
    {
        this.upgradeSystem = upgradeSystem;
        this.money = upgradeSystem.GetMoney();
    }

    public void Execute()
    {
        GameObject selected = upgradeSystem.GetSelectedTower();
        if (selected == null) return;

        site = selected.GetComponentInParent<ConstructionSide>();
        if (site == null) return;

        GameObject tower = site.GetPlacedTower();
        if (tower == null) return;

        var price = tower.GetComponent<TowerPrice>();
        var data = tower.GetComponent<TowerRuntimeData>();

        oldLevel = price.GetLevel();
        oldPrefab = data.prefab;
        cost = price.GetUpgradeCost();

        upgradeSystem.UpgradeCurrent(); // zmienia site

        GameObject upgraded = site.GetPlacedTower();
        var newPrice = upgraded.GetComponent<TowerPrice>();
        var newData = upgraded.GetComponent<TowerRuntimeData>();

        newLevel = newPrice.GetLevel();
        newPrefab = newData.prefab;
    }

    public void Undo()
    {
        GameObject current = site.GetPlacedTower();
        if (current == null) return;

        Object.Destroy(current);

        GameObject restored = Object.Instantiate(oldPrefab);
        restored.transform.SetParent(site.transform);
        restored.transform.localPosition = Vector3.zero;

        var data = restored.GetComponent<TowerRuntimeData>() ?? restored.AddComponent<TowerRuntimeData>();
        data.prefab = oldPrefab;

        var price = restored.GetComponent<TowerPrice>();
        price.SetLevel(oldLevel);

        site.SetTower(restored);

        money.AddMoney(cost);

        upgradeSystem.SetSelectedTower(restored);
        upgradeSystem.RefreshRange();
    }

    public void Redo()
    {
        GameObject current = site.GetPlacedTower();
        if (current != null)
            Object.Destroy(current);

        GameObject upgraded = Object.Instantiate(newPrefab);
        upgraded.transform.SetParent(site.transform);
        upgraded.transform.localPosition = Vector3.zero;

        var data = upgraded.GetComponent<TowerRuntimeData>() ?? upgraded.AddComponent<TowerRuntimeData>();
        data.prefab = newPrefab;

        var price = upgraded.GetComponent<TowerPrice>();
        price.SetLevel(newLevel);

        site.SetTower(upgraded);

        money.SubMoney(cost);

        upgradeSystem.SetSelectedTower(upgraded);
        upgradeSystem.RefreshRange();
    }
}