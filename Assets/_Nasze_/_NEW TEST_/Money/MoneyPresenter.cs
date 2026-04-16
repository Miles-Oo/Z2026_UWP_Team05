public class MoneyPresenter
{
    private MoneyModel _model;
    private MoneyView _view;

    public MoneyPresenter(MoneyModel model, MoneyView view)
    {
        _model = model;
        _view = view;

        _model.OnMoneyChanged += UpdateView;
        UpdateView();
    }

    private void UpdateView()
    {
        _view.SetMoney(_model.CurrMoney);
    }

    public void AddMoney(int value)
    {
        _model.AddMoney(value);
    }

    public void SubMoney(int value)
    {
        _model.SubMoney(value);
    }

    public void Dispose()
    {
        _model.OnMoneyChanged -= UpdateView;
    }
}