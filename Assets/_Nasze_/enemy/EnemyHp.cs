using UnityEngine;
using System;

public class EnemyHp : MonoBehaviour
{
    private EnemyAI enemyAI;
    [SerializeField] private int _currHp;
    [SerializeField] private int _maxHp;

    public event Action OnEnemyDeath; // nowy event

    void Start()
    {
        enemyAI = GetComponent<EnemyAI>();
        if (_maxHp <= 0) _maxHp = 1;
        _currHp = _maxHp;
    }

    public void SubHp(int hp)
    {
        _currHp -= hp;
        if (_currHp <= 0)
        {
            _currHp = 0;
            EndOfLife();
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