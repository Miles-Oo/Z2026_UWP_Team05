using UnityEngine;
using TMPro;

public class enemyUiInNextWave : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _waveText;
    [SerializeField] private TextMeshProUGUI _enemyText;
    [SerializeField] private WaveManager _waveManager;

    private void Start()
    {
        if (_waveManager == null)
        {
            Debug.LogError("WaveManager nie jest ustawiony w UI!");
            return;
        }

        UpdateUI();
    }

    private void UpdateUI()
    {
        UpdateWaveText();
        UpdateEnemyText();
    }

    private int GetNextWaveIndex()
    {
        return _waveManager.currentWaveNumber; // bo currentWaveNumber jest 1-based
    }

    private EnemyWave GetNextWave()
    {
        int index = GetNextWaveIndex();

        if (_waveManager.waves == null)
            return null;

        if (index < 0 || index >= _waveManager.waves.Length)
            return null;

        return _waveManager.waves[index];
    }

    private void UpdateEnemyText()
    {
        EnemyWave wave = GetNextWave();
        if (wave == null)
        {
            _enemyText.text = "No next wave";
            return;
        }

        int totalEnemiesInWave = 0;

        foreach (var entry in wave.enemies)
        {
            totalEnemiesInWave += entry.count;
        }

        _enemyText.text = $"Enemies next wave: {totalEnemiesInWave}";
    }

    private void UpdateWaveText()
    {
        EnemyWave wave = GetNextWave();
        if (wave == null)
        {
            _waveText.text = "No next wave";
            return;
        }

        string buildString = "";

        foreach (var enemy in wave.enemies)
        {
            buildString += enemy.enemyName + " ";
        }

        _waveText.text = buildString;
    }
}