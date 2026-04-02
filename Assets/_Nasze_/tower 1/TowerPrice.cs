using UnityEngine;

public class TowerPrice : MonoBehaviour
{
    [SerializeField] private int price;
    [SerializeField] private int upgradeCost = 30;
    [SerializeField] private GameObject nextLevelPrefab;
    private int level = 1;

    public int GetPrice() => price;

    public int GetUpgradeCost() => upgradeCost;

    public int GetLevel() => level;

    public GameObject GetNextLevelPrefab() => nextLevelPrefab;

    public void LevelUp()
    {
        if (level >= 4) return;
        level++;
    }

    public void SetNextLevelPrefab(GameObject prefab)
    {
        nextLevelPrefab = prefab;
    }
}