using UnityEngine;

public class CommandUpgrade : ICommand, ICommandWithHistory
{
    private TowerUpgrade upgradeSystem;
    private Money money;

    private ConstructionSide site;

    private GameObject oldPrefab;
    private GameObject newPrefab;
    private int oldLevel;
    private int newLevel;
    private int cost;

    private bool isSlowTower;

    public bool WasSkipped { get; private set; }

    public CommandUpgrade(TowerUpgrade upgradeSystem)
    {
        this.upgradeSystem = upgradeSystem;
        this.money = upgradeSystem.GetMoney();
    }

    public void Execute()
    {
        WasSkipped = false;

        GameObject selected = upgradeSystem.GetSelectedTower();
        if (selected == null) { WasSkipped = true; return; }

        site = selected.GetComponentInParent<ConstructionSide>();
        if (site == null) { WasSkipped = true; return; }

        GameObject tower = site.GetPlacedTower();
        if (tower == null) { WasSkipped = true; return; }

        var slow = tower.GetComponent<SlowTowerController>();

        if (slow != null)
        {
            isSlowTower = true;

            if (!slow.CanUpgrade())
            {
                WasSkipped = true;
                return;
            }

            cost = slow.GetUpgradeCost();

            if (money.GetCurrMoney() < cost)
            {
                WasSkipped = true;
                return;
            }

            oldLevel = slow.GetCurrentLevel();

            money.SubMoney(cost);
            slow.Upgrade();

            newLevel = slow.GetCurrentLevel();

            upgradeSystem.SetSelectedTower(tower);
            upgradeSystem.RefreshRangeDelayed();

            return;
        }

        isSlowTower = false;

        var price = tower.GetComponent<TowerPrice>();
        var data = tower.GetComponent<TowerRuntimeData>();

        if (price == null || data == null)
        {
            WasSkipped = true;
            return;
        }

        if (price.GetLevel() >= 4 || price.GetNextLevelPrefab() == null)
        {
            WasSkipped = true;
            return;
        }

        cost = price.GetUpgradeCost();

        if (money.GetCurrMoney() < cost)
        {
            WasSkipped = true;
            return;
        }

        oldLevel = price.GetLevel();
        oldPrefab = data.prefab;

        upgradeSystem.UpgradeCurrent();

        GameObject upgraded = site.GetPlacedTower();
        if (upgraded == null)
        {
            WasSkipped = true;
            return;
        }

        var newPrice = upgraded.GetComponent<TowerPrice>();
        var newData = upgraded.GetComponent<TowerRuntimeData>();

        newLevel = newPrice.GetLevel();
        newPrefab = newData.prefab;
    }

    public void Undo()
    {
        if (WasSkipped) return;
        if (site == null) return;

        GameObject tower = site.GetPlacedTower();
        if (tower == null) return;

        if (isSlowTower)
        {
            var slow = tower.GetComponent<SlowTowerController>();
            slow.SetLevel(oldLevel);

            money.AddMoney(cost);

            upgradeSystem.SetSelectedTower(tower);
            upgradeSystem.RefreshRangeDelayed();
            return;
        }

        Object.Destroy(tower);

        GameObject restored = Object.Instantiate(oldPrefab);
        restored.transform.SetParent(site.transform);
        restored.transform.localPosition = Vector3.zero;

        var data = restored.GetComponent<TowerRuntimeData>()
                   ?? restored.AddComponent<TowerRuntimeData>();

        data.prefab = oldPrefab;

        var price = restored.GetComponent<TowerPrice>();
        price.SetLevel(oldLevel);

        site.SetTower(restored);

        money.AddMoney(cost);

        upgradeSystem.SetSelectedTower(restored);
        upgradeSystem.RefreshRangeDelayed();
    }

    public void Redo()
    {
        if (WasSkipped) return;
        if (site == null) return;

        GameObject tower = site.GetPlacedTower();
        if (tower == null) return;

        if (isSlowTower)
        {
            var slow = tower.GetComponent<SlowTowerController>();
            slow.SetLevel(newLevel);

            money.SubMoney(cost);

            upgradeSystem.SetSelectedTower(tower);
            upgradeSystem.RefreshRangeDelayed();
            return;
        }

        Object.Destroy(tower);

        GameObject upgraded = Object.Instantiate(newPrefab);
        upgraded.transform.SetParent(site.transform);
        upgraded.transform.localPosition = Vector3.zero;

        var data = upgraded.GetComponent<TowerRuntimeData>()
                   ?? upgraded.AddComponent<TowerRuntimeData>();

        data.prefab = newPrefab;

        var price = upgraded.GetComponent<TowerPrice>();
        price.SetLevel(newLevel);

        site.SetTower(upgraded);

        money.SubMoney(cost);

        upgradeSystem.SetSelectedTower(upgraded);
        upgradeSystem.RefreshRangeDelayed();
    }
}