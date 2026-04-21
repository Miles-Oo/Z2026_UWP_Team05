using UnityEngine;

public class CommandUpgrade : ICommand
{
    private TowerUpgrade upgradeSystem;
    private Money money;

    private GameObject oldTower;
    private GameObject newTower;

    private GameObject oldPrefab;
    private GameObject newPrefab;

    private int oldLevel;
    private int newLevel;

    private int cost;

    private ConstructionSide site;

    public CommandUpgrade(TowerUpgrade upgradeSystem)
    {
        this.upgradeSystem = upgradeSystem;
        this.money = upgradeSystem.GetMoney();
    }

    public void Execute()
    {
        oldTower = upgradeSystem.GetSelectedTower();
        if (oldTower == null) return;

        var oldPrice = oldTower.GetComponent<TowerPrice>();
        var oldData = oldTower.GetComponent<TowerRuntimeData>();

        site = oldTower.GetComponentInParent<ConstructionSide>();

        oldLevel = oldPrice.GetLevel();
        cost = oldPrice.GetUpgradeCost();
        oldPrefab = oldData.prefab;

        upgradeSystem.UpgradeCurrent();

        newTower = upgradeSystem.GetSelectedTower();

        var newPrice = newTower.GetComponent<TowerPrice>();
        var newData = newTower.GetComponent<TowerRuntimeData>();

        newLevel = newPrice.GetLevel();
        newPrefab = newData.prefab;
    }

    public void Undo()
    {
        if (site == null || oldPrefab == null) return;

        if (newTower != null)
            Object.Destroy(newTower);

        GameObject restored = Object.Instantiate(oldPrefab);

        restored.transform.SetParent(site.transform);
        restored.transform.localPosition = Vector3.zero;

        var data = restored.GetComponent<TowerRuntimeData>();
        if (data == null)
            data = restored.AddComponent<TowerRuntimeData>();

        data.prefab = oldPrefab;

        var price = restored.GetComponent<TowerPrice>();
        price.SetLevel(oldLevel);

        site.SetTower(restored);
        upgradeSystem.SetSelectedTower(restored);
        upgradeSystem.RefreshRange();
        money.AddMoney(cost);
    }

    public void Redo()
    {
        if (site == null || newPrefab == null) return;

        GameObject current = site.GetPlacedTower();
        if (current != null)
            Object.Destroy(current);

        GameObject upgraded = Object.Instantiate(newPrefab);
        upgraded.transform.SetParent(site.transform);
        upgraded.transform.localPosition = Vector3.zero;

        var data = upgraded.GetComponent<TowerRuntimeData>() ??
                upgraded.AddComponent<TowerRuntimeData>();
        data.prefab = newPrefab;

        var price = upgraded.GetComponent<TowerPrice>();
        price.SetLevel(newLevel);

        site.SetTower(upgraded);

        money.SubMoney(cost);

        upgradeSystem.SetSelectedTower(upgraded);
        upgradeSystem.RefreshRange();
    }
}