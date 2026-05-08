using UnityEngine;

public static class TowerFactory
{
    public static GameObject GetPrefab(TowerType type)
    {
        GameObject prefab = type switch
        {
            TowerType.Basic => GameAssets.Instance.basicTowerPrefab,
            TowerType.Slow  => GameAssets.Instance.slowTowerPrefab,
            _ => null
        };

        Debug.Log($"[FACTORY] GetPrefab called → {type} → {prefab?.name}");

        return prefab;
    }
}