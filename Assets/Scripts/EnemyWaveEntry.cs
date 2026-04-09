using UnityEngine;

[System.Serializable]
public class EnemyWaveEntry
{
    public GameObject enemyPrefab; // rodzaj wroga
    public int count;              // ile spawnów tego wroga
}

[System.Serializable]
public class EnemyWave
{
    public string waveName;                // nazwa fali (opcjonalnie)
    public EnemyWaveEntry[] enemies;       // lista różnych wrogów w tej fali
}