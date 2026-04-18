using UnityEngine;

public class WavePresenter : MonoBehaviour
{
    [SerializeField] private WaveManager model;
    [SerializeField] private UiCurrWaveView view;

    void Start()
    {
        model.OnWaveChanged += UpdateWave;

        UpdateWave();
    }

    void OnDestroy()
    {
        model.OnWaveChanged -= UpdateWave;
    }

    void UpdateWave()
    {
        view.SetWave(model.currentWaveNumber, model.TotalWaves);
    }

}