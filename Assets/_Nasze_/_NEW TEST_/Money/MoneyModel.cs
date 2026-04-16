using System;

public class MoneyModel
{
    private int _currMoney;

    public int CurrMoney => _currMoney;

    public event Action OnMoneyChanged;

    public MoneyModel(int startMoney)
    {
        _currMoney = Math.Max(0, startMoney);
    }

    public void AddMoney(int value)
    {
        _currMoney += value;
        OnMoneyChanged?.Invoke();
    }

    public void SubMoney(int value)
    {
        _currMoney = Math.Max(0, _currMoney - value);
        OnMoneyChanged?.Invoke();
    }
}