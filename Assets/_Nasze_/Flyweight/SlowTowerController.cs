using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowTowerController : MonoBehaviour, IRangeProvider
{
    [SerializeField] private float knockbackAttackInterval = 5f;
    [SerializeField] private SlowTowerStats[] levels;

    private int currentLevel = 0;
    private List<EnemyHp> enemies = new();

    private bool initialized;
    private Coroutine attackRoutine;

    public SlowTowerStats CurrentStats => levels[currentLevel];

    private float range;
    private enum SlowTowerMode
    {
        Slow,
        Knockback
    }

    private SlowTowerMode mode = SlowTowerMode.Slow;

    public void SetMode(bool useKnockback)
    {
        mode = useKnockback
            ? SlowTowerMode.Knockback
            : SlowTowerMode.Slow;

        StopAllCoroutines();
        attackRoutine = StartCoroutine(AttackLoop());
    }

    public string GetModeName()
    {
        return mode.ToString();
    }

    public bool IsKnockbackMode()
    {
        return mode == SlowTowerMode.Knockback;
    }

    public float GetKnockbackAttackInterval()
    {
        return knockbackAttackInterval;
    }

    public float GetKnockbackValue()
    {
        return CurrentStats.slowPercent * 10f;
    }

    public float GetRange()
    {
        return range;
    }

    void Start()
    {
        if (initialized) return;
        initialized = true;

        if (levels == null || levels.Length == 0)
        {
            return;
        }

        ApplyLevel();

        attackRoutine = StartCoroutine(AttackLoop());
    }

    public void Upgrade()
    {
        if (levels == null || levels.Length == 0)
            return;

        if (currentLevel >= levels.Length - 1)
        {
            Debug.Log("MAX LEVEL");
            return;
        }

        currentLevel++;

        ApplyLevel();
        var data = GetComponent<TowerRuntimeData>();
        if (data != null)
            data.level = currentLevel + 1;
    }

    private void ApplyLevel()
    {
        if (levels == null || levels.Length == 0)
            return;

        var stats = CurrentStats;

        range = stats.range * transform.lossyScale.x;

        var col = GetComponent<SphereCollider>();

        if (col != null)
        {
            col.radius = stats.range;
        }

        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            Destroy(transform.GetChild(i).gameObject);
        }

        if (stats.modelPrefab != null)
        {
            var model = Instantiate(stats.modelPrefab, transform);

            model.transform.localPosition = Vector3.zero;
            model.transform.localRotation = Quaternion.identity;
            model.transform.localScale = Vector3.one;
        }
    }

    public void SetLevel(int level)
    {
        level = Mathf.Clamp(level, 1, levels.Length);

        currentLevel = level - 1;

        ApplyLevel();
    }

    public int GetUpgradeCost()
    {
        if (levels == null || levels.Length == 0)
            return 0;

        if (currentLevel >= levels.Length - 1)
            return 0;

        return CurrentStats.upgradeCost;
    }

    private IEnumerator AttackLoop()
    {
        while (true)
        {
            if (levels == null || levels.Length == 0)
                yield break;
            
            if (mode == SlowTowerMode.Knockback)
                yield return new WaitForSeconds(knockbackAttackInterval);
            else
                yield return new WaitForSeconds(CurrentStats.attackInterval);

            yield return new WaitForSeconds(CurrentStats.attackInterval);

            if (enemies.Count == 0)
                continue;

             if (mode == SlowTowerMode.Slow)
                DoSlowAttack();
            else
                DoKnockbackAttack();
        }
    }

    private void DoSlowAttack()
    {
        var stats = CurrentStats;

        int dmg = stats.damage;
        float slow = stats.slowPercent;
        float slowTime = stats.slowDuration;

        for (int i = enemies.Count - 1; i >= 0; i--)
        {
            var enemy = enemies[i];

            if (enemy == null || !enemy.gameObject.activeInHierarchy)
            {
                enemies.RemoveAt(i);
                continue;
            }

            enemy.SubHp(dmg);

            if (enemy.cachedMovement == null)
                enemy.cachedMovement = enemy.GetComponent<EnemyMovement>();

            if (enemy.cachedMovement != null)
            {
                enemy.cachedMovement.ApplySlow(slow, slowTime);
            }
        }
    }
/// <summary>
/// AOE KNOCKBACK FUNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNN 
/// </summary>
    // private void DoKnockbackAttack()
    // {
    //     var stats = CurrentStats;

    //     int dmg = stats.damage;

    //     float knockbackForce = stats.slowPercent;

    //     for (int i = enemies.Count - 1; i >= 0; i--)
    //     {
    //         var enemy = enemies[i];

    //         if (enemy == null || !enemy.gameObject.activeInHierarchy)
    //         {
    //             enemies.RemoveAt(i);
    //             continue;
    //         }

    //         enemy.SubHp(dmg);

    //         Vector3 dir = (enemy.transform.position - transform.position).normalized;

    //         enemy.transform.position += dir * knockbackForce;

    //         Debug.Log($"[SlowTower] Knockback applied -> {enemy.name}");
    //     }
    // }

    private void DoKnockbackAttack()
    {
               var stats = CurrentStats;

        int dmg = stats.damage;
        float slow = stats.slowPercent * 10;
        float slowTime = stats.slowDuration;

        for (int i = enemies.Count - 1; i >= 0; i--)
        {
            var enemy = enemies[i];

            if (enemy == null || !enemy.gameObject.activeInHierarchy)
            {
                enemies.RemoveAt(i);
                continue;
            }

            enemy.SubHp(dmg);

            if (enemy.cachedMovement == null)
                enemy.cachedMovement = enemy.GetComponent<EnemyMovement>();

            if (enemy.cachedMovement != null)
            {
                enemy.cachedMovement.ApplySlow(slow, slowTime);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        var enemy = other.GetComponent<EnemyHp>();

        if (enemy != null && !enemies.Contains(enemy))
        {
            enemies.Add(enemy);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var enemy = other.GetComponent<EnemyHp>();

        if (enemy != null)
        {
            enemies.Remove(enemy);
        }
    }

    public int GetCurrentLevel()
    {
        return currentLevel + 1;
    }

    public bool CanUpgrade()
    {
        return currentLevel < levels.Length - 1;
    }
}