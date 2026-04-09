using UnityEngine;
using System.Collections;
public class EnemyAttack : MonoBehaviour
{

    private int damage=2;
    private EnemyAI enemyAI;
    bool isAttacking =false;
    public bool IsAttacking(){return isAttacking;}
    // Start is called once before the first execution of Update after the MonoBehaviour is created
   void Start()
    {
        enemyAI=GetComponent<EnemyAI>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    IEnumerator AttackCoroutine(){

        isAttacking=true;
    while (enemyAI.GetBase() != null){
            if ( enemyAI.GetBase() != null)
            {
                Debug.Log("Atak na bazę!");
                AttackBase();
            }
            yield return new WaitForSeconds(2);
        }
    }
  public void StartAttacking()
{
    if (isAttacking) return;
    StartCoroutine(AttackCoroutine());
}
    private void AttackBase()
    {
        enemyAI.GetBase().GetComponent<baseHp>().SubHp(damage);
    }
}
