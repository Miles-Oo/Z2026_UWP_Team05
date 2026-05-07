using UnityEngine;
using System.Collections;

public class EnemyMovement : MonoBehaviour
{
    private Transform[] waypoints;
    [SerializeField] Transform transformAsset;
    public float speed = 120f;
    private float currentSpeed;
    private Coroutine slowCoroutine;
    private int currentWaypoint = 0;
    private EnemyAI enemyAI;

    void Start()
    {
        enemyAI = GetComponent<EnemyAI>();
        currentSpeed = speed;
    }

    public void SetWaypoints(Transform[] newWaypoints)
    {
        waypoints = newWaypoints;
    }

    void Update()
    {
        if (waypoints == null || waypoints.Length == 0) return;
        if (currentWaypoint < waypoints.Length){
            Transform target = waypoints[currentWaypoint];
            transform.position = Vector3.MoveTowards(transform.position,target.position,currentSpeed * Time.deltaTime);
            transformAsset.LookAt(target);
            if (Vector3.Distance(transform.position, target.position) < 0.1f){
            currentWaypoint++; }
        }
        else{
            if (!enemyAI.GetEnemyAttack().IsAttacking()){enemyAI.GetEnemyAttack().StartAttacking();}
        }
    }
    public void ApplySlow(float slowPercent, float duration)
    {
        if (slowCoroutine != null)
            StopCoroutine(slowCoroutine);

        slowCoroutine = StartCoroutine(SlowRoutine(slowPercent, duration));
    }

    private IEnumerator SlowRoutine(float slowPercent, float duration)
    {
        currentSpeed = speed * (1f - slowPercent);

        yield return new WaitForSeconds(duration);

        currentSpeed = speed;
        slowCoroutine = null;
    }
}