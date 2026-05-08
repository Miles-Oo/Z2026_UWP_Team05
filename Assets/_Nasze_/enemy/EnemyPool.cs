using System.Collections.Generic;
using UnityEngine;

public class EnemyPool : MonoBehaviour
{
    public static EnemyPool Instance;

    [SerializeField] private int initialSize = 10;

    private Dictionary<EnemyType, Queue<GameObject>> pools = new Dictionary<EnemyType, Queue<GameObject>>();

    private void Awake()
    {
        Instance = this;
    }

    public GameObject Get(EnemyType type, Vector3 position)
    {
        if (!pools.ContainsKey(type))
        {
            pools[type] = new Queue<GameObject>();

            Prewarm(type);
        }

        GameObject enemy;

        if (pools[type].Count > 0)
        {
            enemy = pools[type].Dequeue();

            Debug.Log($"[POOL] Reusing enemy {enemy.name}");
        }
        else
        {
            enemy = CreateNew(type);

            Debug.Log($"[POOL] Creating NEW enemy {enemy.name}");
        }

        enemy.transform.position = position;
        enemy.transform.rotation = Quaternion.identity;

        enemy.SetActive(true);

        return enemy;
    }

    public void Return(EnemyType type, GameObject enemy)
    {
        Collider col = enemy.GetComponent<Collider>();

        if (col != null)
        {
            col.enabled = false;
            col.enabled = true;
        }

        enemy.SetActive(false);

        pools[type].Enqueue(enemy);

        Debug.Log($"[POOL] Returned {enemy.name}");
    }

    private void Prewarm(EnemyType type)
    {
        for (int i = 0; i < initialSize; i++)
        {
            GameObject enemy = CreateNew(type);

            enemy.SetActive(false);

            pools[type].Enqueue(enemy);
        }
    }

    private GameObject CreateNew(EnemyType type)
    {
        GameObject prefab = EnemyFactory.GetPrefab(type);

        IEnemyPrototype prototype = prefab.GetComponent<IEnemyPrototype>();

        GameObject enemy = prototype.Clone(Vector3.zero);

        enemy.transform.SetParent(transform);

        EnemyPoolObject poolObj = enemy.GetComponent<EnemyPoolObject>();

        if (poolObj == null)
            poolObj = enemy.AddComponent<EnemyPoolObject>();

        poolObj.enemyType = type;

        return enemy;
    }
}