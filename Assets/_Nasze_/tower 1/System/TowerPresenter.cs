using UnityEngine;

public class TowerPresenter
{
    private TowerModel model;
    private TowerUpgradeUI view;
    private TowerUpgrade upgrade;

    public TowerPresenter(TowerModel model, TowerUpgradeUI view, TowerUpgrade upgrade)
    {
        this.model = model;
        this.view = view;
        this.upgrade = upgrade;

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
        upgrade.UpgradeCurrent();

        GameObject newTower = upgrade.GetSelectedTower();

        model = new TowerModel(newTower);

        Refresh();
    }

    public void Remove()
    {
        view.Unbind();
    }
}