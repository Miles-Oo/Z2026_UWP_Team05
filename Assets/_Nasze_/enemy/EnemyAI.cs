using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    GameObject baseTarget;

    private EnemyAttack enemyAttack;
    private EnemyHp enemyHp;
    private EnemyMovement enemyMovement;

    public EnemyAttack GetEnemyAttack() => enemyAttack;
    public GameObject GetBase() => baseTarget;

    public void SetBase(GameObject baseObj)
    {
        baseTarget = baseObj;
    }

    public void GiveBaseMoney()
    {
        baseTarget.GetComponent<Money>().AddMoney(10);
    }

    void Start()
    {
        enemyAttack = GetComponent<EnemyAttack>();
        enemyHp = GetComponent<EnemyHp>();
        enemyMovement = GetComponent<EnemyMovement>();

        // 🔥 KLUCZOWE POŁĄCZENIE
        enemyMovement.OnReachedEnd += HandleReachedEnd;
    }

    private void HandleReachedEnd()
    {
        enemyAttack.StartAttacking();
    }

    void OnDestroy()
    {
        enemyMovement.OnReachedEnd -= HandleReachedEnd;
    }
}