using UnityEngine;

[CreateAssetMenu(menuName = "Flyweight/SlowTowerStats")]
public class SlowTowerStats : ScriptableObject
{
    public int damage;
    public float range;
    public float attackInterval;

    [Header("Slow effect")]
    public float slowPercent;
    public float slowDuration;
    public GameObject modelPrefab;
    public Texture nextUpgradePreview;
    public int upgradeCost;
}