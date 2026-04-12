using UnityEngine;
using System.Collections;

public class EnemyAttack : MonoBehaviour
{
    private int damage = 2;
    private EnemyAI enemyAI;

    private bool isAttacking = false;

    public float rotationSpeed = 3600f;
    [SerializeField] private Transform transformAsset;

    void Start()
    {
        enemyAI = GetComponent<EnemyAI>();
    }

    void Update()
    {
        if (isAttacking)
            transformAsset.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
    }

    IEnumerator AttackCoroutine()
    {
        isAttacking = true;

        while (true)
        {
            var baseObj = enemyAI.GetBase();
            if (baseObj == null) break;

            var bridge = baseObj.GetComponent<HpModelBridge>();
            if (bridge == null || bridge.GetModel() == null) break;

            Debug.Log("Atak na bazę!");

            bridge.GetModel().SubHp(damage);

            yield return new WaitForSeconds(2f);
        }

        isAttacking = false;
    }

    public void StartAttacking()
    {
        if (isAttacking) return;
        StartCoroutine(AttackCoroutine());
    }
}