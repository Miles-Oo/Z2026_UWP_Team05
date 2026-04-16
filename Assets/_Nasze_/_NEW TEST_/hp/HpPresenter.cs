public class HpPresenter
{
    private HpModel _model;
    private HpView _view;

    public HpPresenter(HpModel model, HpView view)
    {
        _model = model;
        _view = view;

        _model.OnHpChanged += UpdateView;

        UpdateView(); // pierwsze odświeżenie
    }

    private void UpdateView()
    {
        _view.SetHp(_model.CurrHp, _model.MaxHp);
    }

    public void AddHp(int value)
    {
        _model.AddHp(value);
    }

    public void SubHp(int value)
    {
        _model.SubHp(value);
    }

    public void Dispose()
    {
        _model.OnHpChanged -= UpdateView;
    }
}