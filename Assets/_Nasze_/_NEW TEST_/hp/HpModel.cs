using System;

public class HpModel
{
    private int _currHp;
    private int _maxHp;

    public int CurrHp => _currHp;
    public int MaxHp => _maxHp;

    public event Action OnHpChanged;

    public HpModel(int maxHp)
    {
        _maxHp = Math.Max(1, maxHp);
        _currHp = _maxHp;
    }

    public void AddHp(int hp)
    {
        _currHp = Math.Min(_currHp + hp, _maxHp);
        OnHpChanged?.Invoke();
    }

    public void SubHp(int hp)
    {
        _currHp = Math.Max(_currHp - hp, 0);
        OnHpChanged?.Invoke();
    }

    public void SetMaxHp(int hp)
    {
        if (hp <= 0) return;

        _maxHp = hp;
        _currHp = Math.Min(_currHp, _maxHp);
        OnHpChanged?.Invoke();
    }
}