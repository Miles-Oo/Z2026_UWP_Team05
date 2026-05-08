using UnityEngine;

[System.Serializable]
public class EnemyWaveEntry
{
    public EnemyType enemyType;
    public int count;
}

[System.Serializable]
public class EnemyWave
{
        public EnemyWaveEntry[] enemies;
}