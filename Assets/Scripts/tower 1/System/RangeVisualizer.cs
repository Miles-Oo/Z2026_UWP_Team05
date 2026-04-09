using UnityEngine;
using System.Collections.Generic;

public class RangeVisualizer : MonoBehaviour
{
    [SerializeField] private GameObject fencePrefab;
    [SerializeField] private float rotationOffsetY = 0f;

    private List<GameObject> spawned = new();

    public void ShowRange(Vector3 center, float radius)
    {
        Clear();

        float circumference = 2 * Mathf.PI * radius;
        float width = Mathf.Max(0.1f, GetFenceWidth());

        int count = Mathf.Max(6, Mathf.RoundToInt(circumference / width));
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

    private float GetFenceWidth()
    {
        var mf = fencePrefab.GetComponentInChildren<MeshFilter>();
        return mf?.sharedMesh != null ? mf.sharedMesh.bounds.size.z : 1f;
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