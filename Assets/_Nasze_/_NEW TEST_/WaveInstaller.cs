using UnityEngine;

public class WaveInstaller : MonoBehaviour
{
    [SerializeField] private WaveView _view;
    [SerializeField] private WaveManager _waveManager;

    private WaveModel _model;
    private WavePresenter _presenter;

    void Start()
    {
        _model = new WaveModel(_waveManager.TotalWaves);
        _presenter = new WavePresenter(_model, _view);

        _waveManager.Init(_model);
    }

    void OnDestroy()
    {
        _presenter.Dispose();
    }
}