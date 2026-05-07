using UnityEngine;
using System.Collections.Generic;

public class RangeVisualizer : MonoBehaviour
{
    [SerializeField] private GameObject fencePrefab;
    [SerializeField] private float rotationOffsetY = 0f;
    private GameObject currentTower;

    private List<GameObject> spawned = new();

    private void OnEnable()
    {
        if (ObserverUpgrade.Instance != null)
            ObserverUpgrade.Instance.TowerUpgraded += OnTowerUpgraded;
    }

    private void OnDisable()
    {
        if (ObserverUpgrade.Instance != null)
            ObserverUpgrade.Instance.TowerUpgraded -= OnTowerUpgraded;
    }

    private void OnTowerUpgraded(GameObject tower)
    {
        var attack = tower.GetComponent<TowerAttack>();

        if (attack != null)
        {
            ShowRange(tower.transform.position, attack.GetRange());
            return;
        }

        var slow = tower.GetComponent<SlowTowerController>();

        if (slow != null)
        {
            ShowRange(tower.transform.position, slow.GetRange());
            return;
        }
    }

    public void ShowRange(Vector3 center, float radius)
    {
        Clear();

        int count = Mathf.Max(24, Mathf.RoundToInt(radius * 8f));

        float angleStep = 360f / count;

        for (int i = 0; i < count; i++)
        {
            float angle = i * angleStep * Mathf.Deg2Rad;

            Vector3 pos = center + new Vector3(
                Mathf.Cos(angle) * radius,
                0f,
                Mathf.Sin(angle) * radius
            );

            var obj = Instantiate(fencePrefab, pos, Quaternion.identity);

            Vector3 dir = (pos - center).normalized;
            Vector3 tangent = new Vector3(-dir.z, 0f, dir.x);

            obj.transform.rotation =
                Quaternion.LookRotation(tangent) *
                Quaternion.Euler(0f, rotationOffsetY, 0f);

            spawned.Add(obj);
        }
    }

    public void ShowRange(GameObject tower, float radius)
{
    if (tower == null)
    {
        Clear();
        currentTower = null;
        return;
    }

    currentTower = tower;
    ShowRange(tower.transform.position, radius);
}

public void ClearSelectionRange(GameObject tower)
{
    if (currentTower == tower)
    {
        Clear();
        currentTower = null;
    }
}

    private float GetFenceWidth()
    {
        var mf = fencePrefab.GetComponentInChildren<MeshFilter>();

        if (mf == null || mf.sharedMesh == null)
            return 1f;

        Vector3 scale = fencePrefab.transform.lossyScale;

        return mf.sharedMesh.bounds.size.z * scale.z;
    }

    public void Clear()
    {
        foreach (var obj in spawned)
        {
            if (obj) Destroy(obj);
        }
        spawned.Clear();
    }
}