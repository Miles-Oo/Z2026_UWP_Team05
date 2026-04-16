using UnityEngine;
using System;

public class EnemyMovement : MonoBehaviour
{
    private Transform[] waypoints;

    [SerializeField] private Transform transformAsset;
    public float speed = 120f;

    private int currentWaypoint = 0;

    public event Action OnReachedEnd;

    public void SetWaypoints(Transform[] newWaypoints)
    {
        waypoints = newWaypoints;
    }

    void Update()
    {
        if (waypoints == null || waypoints.Length == 0) return;

        if (currentWaypoint < waypoints.Length)
        {
            Transform target = waypoints[currentWaypoint];

            transform.position = Vector3.MoveTowards(
                transform.position,
                target.position,
                speed * Time.deltaTime
            );

            transformAsset.LookAt(target);

            if (Vector3.Distance(transform.position, target.position) < 0.1f)
            {
                currentWaypoint++;
            }
        }
        else
        {
            OnReachedEnd?.Invoke();
            enabled = false;
        }
    }
}