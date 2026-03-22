using UnityEngine;
using System;
using System.Collections;

public class WaveManager : MonoBehaviour
{
    [SerializeField] private EnemyWave[] waves;             
    [SerializeField] private EnemySpawner enemySpawner;     
    [SerializeField] private float spawnInterval = 1f;      
    [SerializeField] private float waveDelay = 3f;          

    public int currentWaveNumber { get; private set; } = 0;
    public int totalEnemiesInWave { get; private set; } = 0;
    public int aliveEnemies { get; private set; } = 0;

    public event Action OnWaveChanged;
    public event Action OnEnemyCountChanged;

    public int TotalWaves => waves.Length;

    private void Start()
    {
        StartCoroutine(StartWaves());
    }

    private IEnumerator StartWaves()
    {
        for (int i = 0; i < waves.Length; i++)
        {
            currentWaveNumber = i + 1;
            EnemyWave wave = waves[i];

            totalEnemiesInWave = 0;
            foreach (var entry in wave.enemies)
            {
                totalEnemiesInWave += entry.count;
            }

            aliveEnemies = totalEnemiesInWave;

            OnWaveChanged?.Invoke();
            OnEnemyCountChanged?.Invoke();

            yield return SpawnWave(wave);

            yield return new WaitUntil(() => aliveEnemies <= 0);

            Debug.Log($"Wave {currentWaveNumber} cleared!");
            yield return new WaitForSeconds(waveDelay);
        }
    }

    private IEnumerator SpawnWave(EnemyWave wave)
    {
        foreach (EnemyWaveEntry entry in wave.enemies)
        {
            for (int i = 0; i < entry.count; i++)
            {
                GameObject enemy = enemySpawner.SpawnEnemy(entry.enemyPrefab);

                EnemyHp enemyHp = enemy.GetComponent<EnemyHp>();
                if (enemyHp != null){
                    enemyHp.OnEnemyDeath += HandleEnemyDeath;
                }

                yield return new WaitForSeconds(spawnInterval);
            }
        }
    }

    private void HandleEnemyDeath()
    {
        aliveEnemies--;
        OnEnemyCountChanged?.Invoke();
        Debug.Log($"Enemy died! {aliveEnemies} remaining in wave {currentWaveNumber}");
    }
}