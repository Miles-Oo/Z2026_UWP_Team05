using UnityEngine;

public class HpInstaller : MonoBehaviour
{
    [SerializeField] private HpView _view;
    [SerializeField] private HpModelBridge _bridge;
    [SerializeField] private int _startHp = 100;

    private HpModel _model;
    private HpPresenter _presenter;

    void Start()
    {
        _model = new HpModel(_startHp);

        _presenter = new HpPresenter(_model, _view);

        _bridge.Init(_model);
    }

    void OnDestroy()
    {
        _presenter.Dispose();
    }
}