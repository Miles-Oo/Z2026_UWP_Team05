using UnityEngine;

public class TowerModel
{
    private TowerPrice price;
    private TowerAttack attack;
    private SlowTowerController slowTower;

    public TowerModel(GameObject tower)
    {
        price = tower.GetComponent<TowerPrice>();
        attack = tower.GetComponent<TowerAttack>();
        slowTower = tower.GetComponent<SlowTowerController>();
    }

    public bool IsSlowTower => slowTower != null;

    public int Level
    {
        get
        {
            if (IsSlowTower)
                return slowTower.GetCurrentLevel();

            return price.GetLevel();
        }
    }

    public int UpgradeCost
    {
        get
        {
            if (IsSlowTower)
                return slowTower.GetUpgradeCost();

            return price.GetUpgradeCost();
        }
    }

    public bool CanUpgrade()
    {
        if (IsSlowTower)
            return slowTower.CanUpgrade();

        return price.GetLevel() < 4 &&
               price.GetNextLevelPrefab() != null;
    }

    public Texture NextTexture
    {
        get
        {
            if (IsSlowTower)
            {
                return slowTower.CurrentStats.nextUpgradePreview;
            }

            return price.GetNextLevelTexture();
        }
    }

    public int Damage
    {
        get
        {
            if (IsSlowTower)
                return slowTower.CurrentStats.damage;

            return attack != null ? attack.GetDamage() : 0;
        }
    }

    public float Range
    {
        get
        {
            if (IsSlowTower)
                return slowTower.GetRange();

            return attack != null ? attack.GetRange() : 0f;
        }
    }

    public TowerPrice Price => price;
}