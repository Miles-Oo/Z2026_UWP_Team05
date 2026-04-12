public class WavePresenter
{
    private WaveModel _model;
    private WaveView _view;

    public WavePresenter(WaveModel model, WaveView view)
    {
        _model = model;
        _view = view;

        _model.OnWaveChanged += UpdateWave;
        _model.OnEnemyChanged += UpdateEnemies;
    }

    private void UpdateWave()
    {
        _view.SetWave(_model.CurrentWave, _model.TotalWaves);
    }

    private void UpdateEnemies()
    {
        _view.SetEnemies(_model.AliveEnemies, _model.TotalEnemies);
    }

    public void Dispose()
    {
        _model.OnWaveChanged -= UpdateWave;
        _model.OnEnemyChanged -= UpdateEnemies;
    }
}