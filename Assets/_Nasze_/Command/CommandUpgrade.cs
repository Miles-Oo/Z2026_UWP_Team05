using UnityEngine;

public class CommandUpgrade : ICommand
{
    private TowerUpgrade upgradeSystem;

    private GameObject towerBefore;
    private GameObject towerAfter;

    private Vector3 position;
    private Quaternion rotation;
    private ConstructionSide site;

    public CommandUpgrade(TowerUpgrade upgradeSystem)
    {
        this.upgradeSystem = upgradeSystem;
    }

    public void Execute()
    {
        towerBefore = upgradeSystem.GetSelectedTower();

        if (towerBefore == null)
            return;

        position = towerBefore.transform.position;
        rotation = towerBefore.transform.rotation;
        site = towerBefore.GetComponentInParent<ConstructionSide>();

        upgradeSystem.UpgradeCurrent();

        towerAfter = upgradeSystem.GetSelectedTower();
    }

    public void Undo()
    {
        if (towerBefore == null || towerAfter == null)
            return;

        if (towerAfter != null)
            Object.Destroy(towerAfter);

        GameObject restored = Object.Instantiate(towerBefore, position, rotation);

        if (site != null)
        {
            restored.transform.SetParent(site.transform);
            restored.transform.localPosition = Vector3.zero;
            site.SetTower(restored);
        }

        upgradeSystem.SetSelectedTower(restored);
    }

    public void Redo()
    {
        Execute();
    }
}