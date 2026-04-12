using UnityEngine;
using TMPro;

public class UiCurrWave : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _waveText;
    [SerializeField] private TextMeshProUGUI _enemyText;
    [SerializeField] private WaveModel _waveModel;

    private void Start()
    {
        if (_waveModel == null)
        {
            Debug.LogError("WaveModel nie jest ustawiony w UI!");
            return;
        }

        _waveModel.OnWaveChanged += UpdateWaveText;
        _waveModel.OnEnemyChanged += UpdateEnemyText;

        UpdateWaveText();
        UpdateEnemyText();
    }

    private void UpdateWaveText()
    {
        _waveText.text = $"Wave: {_waveModel.CurrentWave} / {_waveModel.TotalWaves}";
    }

    private void UpdateEnemyText()
    {
        if (_waveModel.AliveEnemies == 0)
        {
            _enemyText.text = "All enemies destroyed";
        }
        else
        {
            _enemyText.text = $"Enemies: {_waveModel.AliveEnemies} / {_waveModel.TotalEnemies}";
        }
    }

    private void OnDestroy()
    {
        _waveModel.OnWaveChanged -= UpdateWaveText;
        _waveModel.OnEnemyChanged -= UpdateEnemyText;
    }
}