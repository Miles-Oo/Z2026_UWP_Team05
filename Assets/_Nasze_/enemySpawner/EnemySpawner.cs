using UnityEngine;
using System.Linq;
public class EnemySpawner : MonoBehaviour
{
    Transform spawnPoint;
    [SerializeField] RoadDex[] pathWaypoints;
    [SerializeField] GameObject baseTarget;
     private Transform[] sortedWaypoints;
    void Awake()
    {
        // sortowanie po stepIndex, a jeśli są duplikaty po kolejności w hierarchii
        sortedWaypoints = pathWaypoints
            .OrderBy(wp => wp.StepIndex)
            .ThenBy(wp => wp.transform.GetSiblingIndex())
            .Select(wp => wp.GetWayPointPos())
            .ToArray();
    }
    void Start()
    {
        spawnPoint=GetComponent<Transform>();
    }
    public GameObject SpawnEnemy(EnemyType type)
    {
        // GameObject prefab = EnemyFactory.GetPrefab(type);
        // // GameObject enemy = Instantiate(enemyPrefab, transform.position, transform.rotation);

        // IEnemyPrototype prototype = prefab.GetComponent<IEnemyPrototype>();
        // GameObject enemy = prototype.Clone(transform.position);
        GameObject enemy = EnemyPool.Instance.Get(type, transform.position);

        EnemyMovement movement = enemy.GetComponent<EnemyMovement>();
        movement.SetWaypoints(sortedWaypoints);

        EnemyAI ai = enemy.GetComponent<EnemyAI>();
        ai.SetBase(baseTarget);
        return enemy;
    }
}