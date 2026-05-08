using UnityEngine;

public static class EnemyFactory
{
    public static GameObject GetPrefab(EnemyType type)
    {
        GameObject prefab = type switch
        {
            EnemyType.Grazyna => GameAssets.Instance.grazynaEnemyPrefab,
            EnemyType.Jola  => GameAssets.Instance.jolaEnemyPrefab,
            _ => null
        };

        Debug.Log($"[ENEMY FACTORY] GetPrefab → {type} → {prefab?.name}");

        return prefab;
    }
}