using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerAttack : MonoBehaviour
{
    [SerializeField] private float attackInterval = 1f;
    [SerializeField] private int damage = 1;

    private float range;
    public float GetRange(){return range;}
    public int GetDamage(){return damage;}

    private List<EnemyHp> enemiesInRange = new List<EnemyHp>();
    private Coroutine attackCoroutine;

void Awake()
{
    var col = GetComponent<SphereCollider>();
    range = col.radius * transform.lossyScale.x;
}

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("widze wejscie");
        EnemyHp enemy = other.GetComponent<EnemyHp>();
        if (enemy != null && !enemiesInRange.Contains(enemy))
        {
            Debug.Log("enemy to");
            enemiesInRange.Add(enemy);
            if (attackCoroutine == null)
                attackCoroutine = StartCoroutine(AttackCoroutine());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        EnemyHp enemy = other.GetComponent<EnemyHp>();
        if (enemy != null)
        {
            enemiesInRange.Remove(enemy);
        }
    }

    private IEnumerator AttackCoroutine()
    {
        while (enemiesInRange.Count > 0)
        {
            enemiesInRange.RemoveAll(e => e == null);
            if (enemiesInRange.Count == 0) break;

            EnemyHp target = enemiesInRange[0];
            target.SubHp(damage);
            Debug.Log("Tower strzela w enemy!");
            yield return new WaitForSeconds(attackInterval);
        }
        attackCoroutine = null;
    }
}