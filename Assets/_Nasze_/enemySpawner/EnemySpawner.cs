using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] Transform spawnPoint;
    [SerializeField] Transform[] pathWaypoints;
    [SerializeField] GameObject baseTarget;

public GameObject SpawnEnemy(GameObject enemyPrefab)
{
    GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);

    EnemyMovement movement = enemy.GetComponent<EnemyMovement>();
    movement.SetWaypoints(pathWaypoints);

    EnemyAI ai = enemy.GetComponent<EnemyAI>();
    ai.SetBase(baseTarget);
    return enemy;
}
}