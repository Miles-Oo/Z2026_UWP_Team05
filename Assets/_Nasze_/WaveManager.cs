using UnityEngine;
using System.Collections;

public class WaveManager : MonoBehaviour
{
    [SerializeField] public EnemyWave[] waves;
    [SerializeField] private EnemySpawner enemySpawner;
    [SerializeField] private float spawnInterval = 1f;
    [SerializeField] private float waveDelay = 3f;

    public int currentWaveNumber { get; private set; } = 0;
    public int totalEnemiesInWave { get; private set; } = 0;
    public int aliveEnemies { get; private set; } = 0;

    public int TotalWaves => waves?.Length ?? 0;

    private WaveModel _model;
    public WaveModel Model => _model;

    public void Init(WaveModel model)
    {
        _model = model;
    }

    private void Start()
    {
        StartCoroutine(StartWavesWhenReady());
    }

    private IEnumerator StartWavesWhenReady()
    {
        // Dajemy installerom jedna klatke na Init().
        yield return null;

        EnsureModel();

        if (enemySpawner == null)
        {
            Debug.LogError("WaveManager: enemySpawner is not assigned.");
            yield break;
        }

        if (waves == null || waves.Length == 0)
        {
            Debug.LogWarning("WaveManager: no waves configured.");
            yield break;
        }

        yield return StartWaves();
    }

    private IEnumerator StartWaves()
    {
        for (int i = 0; i < waves.Length; i++)
        {
            currentWaveNumber = i + 1;
            EnemyWave wave = waves[i];

            totalEnemiesInWave = 0;
            foreach (EnemyWaveEntry entry in wave.enemies)
            {
                totalEnemiesInWave += entry.count;
            }

            aliveEnemies = totalEnemiesInWave;
            _model?.StartWave(currentWaveNumber, totalEnemiesInWave);

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
                if (enemyHp != null)
                {
                    enemyHp.OnEnemyDeath += HandleEnemyDeath;
                }

                yield return new WaitForSeconds(spawnInterval);
            }
        }
    }

    private void HandleEnemyDeath()
    {
        aliveEnemies = Mathf.Max(0, aliveEnemies - 1);
        _model?.EnemyDied();

        Debug.Log($"Enemy died! {aliveEnemies} remaining in wave {currentWaveNumber}");
    }

    private void EnsureModel()
    {
        if (_model != null)
        {
            return;
        }

        _model = new WaveModel(TotalWaves);
        Debug.LogWarning("WaveManager: WaveModel was not provided via Init(). Created fallback model.");
    }
}
