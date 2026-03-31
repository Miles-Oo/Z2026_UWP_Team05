using UnityEngine;
using System;

public class EnemyHp : MonoBehaviour
{
    private EnemyAI enemyAI;
    [SerializeField] private int _currHp;
    [SerializeField] private int _maxHp;

    public event Action OnEnemyDeath;
    public event Action OnChangeHp;
    public int GetCurrHp(){return _currHp;}
    public int GetMaxHp(){return _maxHp;}
    void Start()
    {
        enemyAI = GetComponent<EnemyAI>();
        if (_maxHp <= 0) _maxHp = 1;
        _currHp = _maxHp;
        OnChangeHp?.Invoke();
    }

public void SubHp(int hp)
{
    _currHp -= hp;

    if (_currHp <= 0)
    {
        _currHp = 0;
        OnChangeHp?.Invoke();
        EndOfLife();
    }
    else
    {
        OnChangeHp?.Invoke();
    }
}

    private void EndOfLife()
    {
        OnEnemyDeath?.Invoke(); // powiadamiamy WaveManager

        if(enemyAI != null && enemyAI.GetBase() != null)
        {
            Money money = enemyAI.GetBase().GetComponent<Money>();
            if(money != null)
            {
                money.AddMoney(19);
            }
        }

        Destroy(gameObject);
    }
}