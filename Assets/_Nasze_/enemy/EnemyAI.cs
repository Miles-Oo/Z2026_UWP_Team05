using UnityEngine;

public class EnemyAI : MonoBehaviour
{


    [SerializeField] GameObject baseTarget;

    private EnemyAttack enemyAttack;
    private EnemyHp enemyHp;
    private EnemyMovement enemyMovement;

    public EnemyAttack GetEnemyAttack(){return enemyAttack;}
    public GameObject GetBase(){return baseTarget;}
    public void SetBase(GameObject baseObj){baseTarget = baseObj;}
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
