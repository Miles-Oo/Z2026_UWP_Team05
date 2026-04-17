using UnityEngine;

public class TowerModel
{
    private TowerPrice price;
    private TowerAttack attack;

    public TowerModel(GameObject tower)
    {
        price = tower.GetComponent<TowerPrice>();
        attack = tower.GetComponent<TowerAttack>();
    }

    public int Level => price.GetLevel();
    public int UpgradeCost => price.GetUpgradeCost();

    public bool CanUpgrade()
    {
        return price.GetLevel() < 4 && price.GetNextLevelPrefab() != null;
    }

    public Texture NextTexture => price.GetNextLevelTexture();

    public int Damage => attack != null ? attack.GetDamage() : 0;
    public float Range => attack != null ? attack.GetRange() : 0f;

    public TowerPrice Price => price;
}