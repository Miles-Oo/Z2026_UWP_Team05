using UnityEngine;

public class TowerPrice : MonoBehaviour
{
    [SerializeField] private int price;
    [SerializeField] private int upgradeCost;
    [SerializeField] private GameObject nextLevelPrefab;
    [SerializeField] private Texture nextLevelTexture;
    [SerializeField] private GameObject prefab;

    private int level = 1;

    public int GetPrice() => price;

    public int GetUpgradeCost() => upgradeCost;

    public int GetLevel() => level;

    public GameObject GetNextLevelPrefab()
    {
        if (level >= 4) return null;
        return nextLevelPrefab;
    }

    public void LevelUp()
    {
        if (level >= 4) return;
        level++;
    }

    public void SetNextLevelPrefab(GameObject prefab)
    {
        nextLevelPrefab = prefab;
    }

    public Texture GetNextLevelTexture()
    {
        return nextLevelTexture;
    }

    public GameObject GetTowerPrefab()
    {
        return prefab;
    }

    public void SetLevel(int targetLevel)
    {
        level = 1;
        for (int i = 1; i < targetLevel; i++)
            LevelUp();
    }
}