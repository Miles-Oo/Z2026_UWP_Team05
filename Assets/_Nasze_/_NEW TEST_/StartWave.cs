using System;

public class WaveModel
{
    public int CurrentWave { get; private set; }
    public int TotalWaves { get; private set; }
    public int AliveEnemies { get; private set; }
    public int TotalEnemies { get; private set; }

    public event Action OnWaveChanged;
    public event Action OnEnemyChanged;

    public WaveModel(int totalWaves)
    {
        TotalWaves = totalWaves;
    }

    public void StartWave(int waveNumber, int totalEnemies)
    {
        CurrentWave = waveNumber;
        TotalEnemies = totalEnemies;
        AliveEnemies = totalEnemies;

        OnWaveChanged?.Invoke();
        OnEnemyChanged?.Invoke();
    }

    public void EnemyDied()
    {
        AliveEnemies = Math.Max(0, AliveEnemies - 1);
        OnEnemyChanged?.Invoke();
    }
}