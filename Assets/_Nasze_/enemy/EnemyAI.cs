using UnityEngine;

public class EnemyAI : MonoBehaviour
{


    GameObject baseTarget;

    private EnemyAttack enemyAttack;
    private EnemyHp enemyHp;
    private EnemyMovement enemyMovement;

    public EnemyAttack GetEnemyAttack(){return enemyAttack;}
    public GameObject GetBase(){return baseTarget;}
    public void SetBase(GameObject baseObj){baseTarget = baseObj;}
    public void GiveBaseMoney(){
        baseTarget.GetComponent<Money>().AddMoney(10);
    }
    void Start()
    {
         enemyAttack =GetComponent<EnemyAttack>();
         enemyHp =GetComponent<EnemyHp>();
         enemyMovement =GetComponent<EnemyMovement>();
    }
    void Update()
    {
        
    }
}
