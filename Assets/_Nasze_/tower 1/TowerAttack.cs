using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerAttack : MonoBehaviour, IRangeProvider
{
    [SerializeField] private float attackInterval = 1f;
    [SerializeField] private int damage = 1;
    [SerializeField] private float manualRange = 0f;

    private float range;
    private IBasicTargetStrategy targetStrategy;
    public float GetRange() => range;
    public int GetDamage() => damage;
    public float GetAttackInterval() => attackInterval;

    public IBasicTargetStrategy GetCurrentStrategy()
    {
        return targetStrategy;
    }
    public string GetStrategyName()
    {
        return targetStrategy.GetType().Name.Replace("EnemyStrategy", "");
    }

    private List<EnemyHp> enemiesInRange = new List<EnemyHp>();
    private Coroutine attackCoroutine;

    void Awake()
    {
        var col = GetComponent<SphereCollider>();

        if (manualRange > 0)
        {
            range = manualRange;
        }
        else if (col != null)
        {
            range = col.radius * transform.lossyScale.x;
        }
        else
        {
            range = 5f;
        }

        if (col != null)
        {
            col.radius = range / transform.lossyScale.x;
        }

        if (targetStrategy == null)
            targetStrategy = new NearestEnemyStrategy();
    }

    void OnTriggerEnter(Collider other)
    {
        EnemyHp enemy = other.GetComponent<EnemyHp>();
        if (enemy != null && !enemiesInRange.Contains(enemy))
        {
            enemiesInRange.Add(enemy);
            if (attackCoroutine == null)
                attackCoroutine = StartCoroutine(AttackCoroutine());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        EnemyHp enemy = other.GetComponent<EnemyHp>();
        if (enemy != null)
            enemiesInRange.Remove(enemy);
    }

    private IEnumerator AttackCoroutine()
    {
        while (enemiesInRange.Count > 0)
        {
            enemiesInRange.RemoveAll(
                e => e == null || !e.gameObject.activeInHierarchy
            );
            if (enemiesInRange.Count == 0) break;

            EnemyHp target = targetStrategy != null
                ? targetStrategy.SelectTarget(enemiesInRange, transform)
                : enemiesInRange[0];
            Debug.Log($"[TowerAttack] Target chosen: {target?.name} using {targetStrategy.GetType().Name}");
            target.SubHp(damage);
            yield return new WaitForSeconds(attackInterval);
        }
        attackCoroutine = null;
    }

    public void SetTargetStrategy(IBasicTargetStrategy strategy)
    {
        targetStrategy = strategy;
        Debug.Log($"[TowerAttack] Strategy changed to: {strategy.GetType().Name}");
    }
}