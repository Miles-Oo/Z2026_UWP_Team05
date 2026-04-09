using UnityEngine;
using TMPro;

public class UiCurrWave : MonoBehaviour
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
        _waveManager.OnWaveChanged += UpdateWaveText;
        _waveManager.OnEnemyCountChanged += UpdateEnemyText;

        UpdateWaveText();
        UpdateEnemyText();
    }

    private void UpdateWaveText()
    {
        _waveText.text = $"Wave: {_waveManager.currentWaveNumber} / {_waveManager.TotalWaves}";
    }

    private void UpdateEnemyText()
    {
        if(_waveManager.aliveEnemies==0){
        _enemyText.text =$"All enemy destroyed";
        }else{
        _enemyText.text = $"Enemies: {_waveManager.aliveEnemies} / {_waveManager.totalEnemiesInWave}";
        }
    }
}