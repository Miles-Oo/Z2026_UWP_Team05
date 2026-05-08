using UnityEngine;
using System;

public class EnemyHp : MonoBehaviour
{
    private EnemyAI enemyAI;
   private int _currHp;
    [SerializeField] private int _maxHp;
    private EnemyValueMoney enemyValue;
    public EnemyMovement cachedMovement;
    public event Action OnEnemyDeath;
    public event Action OnChangeHp;
    private bool isDead;
    public int GetCurrHp(){return _currHp;}
    public int GetMaxHp(){return _maxHp;}
    void Start()
    {
        enemyAI = GetComponent<EnemyAI>();
        enemyValue=GetComponent<EnemyValueMoney>();
        if (_maxHp <= 0) _maxHp = 1;
        _currHp = _maxHp;
        OnChangeHp?.Invoke();
        ObserverEnemyAudio.Instance?.Register(this);
    }

    private void OnEnable()
    {
        isDead = false;

        _currHp = _maxHp;

        OnChangeHp?.Invoke();
    }

    // private void OnDisable()
    // {
    //     StopAllCoroutines();
    // }

public void SubHp(int hp)
{
    if (isDead) return;
    _currHp -= hp;

    if (_currHp <= 0)
    {
        isDead = true;
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
                money.AddMoney(enemyValue.GetValue());
            }
        }

        // Destroy(gameObject);
        EnemyPoolObject poolObj =
            GetComponent<EnemyPoolObject>();

        EnemyPool.Instance.Return(
            poolObj.enemyType,
            gameObject
        );
    }

    private void OnDestroy()
    {
        ObserverEnemyAudio.Instance?.Unregister(this);
    }
}