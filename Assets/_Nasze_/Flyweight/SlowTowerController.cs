using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowTowerController : MonoBehaviour, IRangeProvider
{
    [SerializeField] private SlowTowerStats[] levels;

    private int currentLevel = 0;
    private List<EnemyHp> enemies = new();

    private bool initialized;
    private Coroutine attackRoutine;

    public SlowTowerStats CurrentStats => levels[currentLevel];

    private float range;

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

            yield return new WaitForSeconds(CurrentStats.attackInterval);

            if (enemies.Count == 0)
                continue;

            DoAoEAttack();
        }
    }

    private void DoAoEAttack()
    {
        var stats = CurrentStats;

        int dmg = stats.damage;
        float slow = stats.slowPercent;
        float slowTime = stats.slowDuration;

        for (int i = enemies.Count - 1; i >= 0; i--)
        {
            var enemy = enemies[i];

            if (enemy == null)
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