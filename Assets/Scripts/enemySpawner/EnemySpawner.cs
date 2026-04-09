using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] Transform spawnPoint;
    [SerializeField] GameObject baseTarget;

    [SerializeField] Transform[] pathWaypoints;

    [SerializeField] float spawnInterval = 6f;

    void Start()
    {
        StartCoroutine(SpawnCoroutine());
    }

    IEnumerator SpawnCoroutine()
    {
        while (true)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

void SpawnEnemy()
{
    GameObject enemy = Instantiate(
        enemyPrefab,
        spawnPoint.position,
        spawnPoint.rotation
    );

    EnemyMovement movement = enemy.GetComponent<EnemyMovement>();
    movement.SetWaypoints(pathWaypoints);

    EnemyAI ai = enemy.GetComponent<EnemyAI>();
    ai.SetBase(baseTarget);
}
}