using UnityEngine;
public class MoneyInstaller : MonoBehaviour
{
    [SerializeField] private MoneyView _view;
    [SerializeField] private MoneyBridge _bridge;
    [SerializeField] private int _startMoney = 0;

    private MoneyModel _model;
    private MoneyPresenter _presenter;

    public MoneyModel Model => _model; // opcjonalnie

    void Start()
    {
        _model = new MoneyModel(_startMoney);
        _presenter = new MoneyPresenter(_model, _view);

        _bridge.Init(_model);
    }

    void OnDestroy()
    {
        _presenter.Dispose();
    }
}