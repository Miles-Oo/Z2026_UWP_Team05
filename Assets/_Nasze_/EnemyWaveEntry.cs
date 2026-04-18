using UnityEngine;

[System.Serializable]
public class EnemyWaveEntry
{
    public GameObject enemyPrefab;
    public int count;
}

[System.Serializable]
public class EnemyWave
{
        public EnemyWaveEntry[] enemies;
}