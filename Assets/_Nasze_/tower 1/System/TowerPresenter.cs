using UnityEngine;

public class TowerPresenter
{
    private TowerModel model;
    private TowerUpgradeUI view;
    private TowerUpgrade upgrade;
    private CommandManager commandManager;

    public TowerPresenter(TowerModel model, TowerUpgradeUI view, TowerUpgrade upgrade, CommandManager commandManager)
    {
        this.model = model;
        this.view = view;
        this.upgrade = upgrade;
        this.commandManager = commandManager;

        view.Bind(this);
        Refresh();
    }

    public void Rebind(GameObject newTower)
    {
        model = new TowerModel(newTower);
        Refresh();
    }

    public void Refresh()
    {
        if (model == null || view == null) return;

        if (!model.CanUpgrade())
        {
            view.ShowMaxLevel();
            return;
        }

        view.UpdateText(model.UpgradeCost);
        view.UpdateImage(model.NextTexture);
    }

    public void OnUpgradeClicked()
    {
        var command = new CommandUpgrade(upgrade);
        commandManager.ExecuteCommand(command);

        GameObject newTower = upgrade.GetSelectedTower();

        if (newTower == null) return;

        model = new TowerModel(newTower);

        Refresh();

    }

    public void Undo()
    {
        commandManager.Undo();
        RefreshAfterUndoRedo();
    }

    public void Redo()
    {
        commandManager.Redo();
        RefreshAfterUndoRedo();
    }

    private void RefreshAfterUndoRedo()
    {
        GameObject tower = upgrade.GetSelectedTower();
        if (tower == null) return;

        model = new TowerModel(tower);
        Refresh();

    }

    public void Remove()
    {
        view.Unbind();
    }
}