using UnityEngine;
using System.Collections;

public class EnemyAttack : MonoBehaviour
{
    private int damage = 2;
    private EnemyAI enemyAI;
    bool isAttacking = false;
    public bool IsAttacking() { return isAttacking; }

    public float rotationSpeed = 3600f; 
    [SerializeField] Transform transformAsset;

    void Start()
    {
        enemyAI = GetComponent<EnemyAI>();
    }

    void Update()
    {
        if (isAttacking)
        { transformAsset.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);}
    }

    IEnumerator AttackCoroutine()
    {
        isAttacking = true;
        while (enemyAI.GetBase() != null)
        {
            if (enemyAI.GetBase() != null)
            {
                Debug.Log("Atak na bazę!");
                AttackBase();
            }
            yield return new WaitForSeconds(2f);
        }
        isAttacking = false;
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