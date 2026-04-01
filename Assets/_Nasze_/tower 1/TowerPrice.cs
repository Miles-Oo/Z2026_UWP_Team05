using UnityEngine;

public class TowerPrice : MonoBehaviour
{
    [SerializeField] private int price;
    [SerializeField] private int upgradeCost = 30;
    [SerializeField] private GameObject nextLevelPrefab; // prefab następnego poziomu
    private int level = 1;

    public int GetPrice() => price;
    public int GetUpgradeCost() => upgradeCost;
    public int GetLevel() => level;

    public GameObject GetNextLevelPrefab() => nextLevelPrefab;

    public void LevelUp()
    {
        if (level < 4) level++;
        upgradeCost += 50;
        Debug.Log($"Tower leveled up to {level}, next upgrade cost: {upgradeCost}");
    }
}