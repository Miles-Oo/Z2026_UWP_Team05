using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerAttack : MonoBehaviour
{
    [SerializeField] private float attackInterval = 1f;
    [SerializeField] private int damage = 1;

    [Header("Manual Range (overrides collider)")]
    [SerializeField] private float manualRange = 0f; // 0 = użyj kolidera

    private float range;
    public float GetRange() => range;
    public int GetDamage() => damage;

    private List<EnemyHp> enemiesInRange = new List<EnemyHp>();
    private Coroutine attackCoroutine;

    void Awake()
    {
        if (manualRange > 0)
        {
            range = manualRange; // ręcznie ustawiony zasięg prefab
        }
        else
        {
            var col = GetComponent<SphereCollider>();
            if (col != null)
                range = col.radius * transform.lossyScale.x;
            else
                range = 5f; // domyślny zasięg
        }
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
            enemiesInRange.RemoveAll(e => e == null);
            if (enemiesInRange.Count == 0) break;

            EnemyHp target = enemiesInRange[0];
            target.SubHp(damage);
            yield return new WaitForSeconds(attackInterval);
        }
        attackCoroutine = null;
    }
}