using UnityEngine;

public class RangeVisualizer : MonoBehaviour
{
    [SerializeField] private GameObject fencePrefab;

    [Header("Opcjonalne poprawki modelu")]
    [SerializeField] private float rotationOffsetY = 0f;

    private GameObject[] spawned;

    public void ShowRange(Vector3 center, float radius)
    {
        Clear();

        float circumference = 2 * Mathf.PI * radius;

        float fenceWidth = GetFenceWidth();
        if (fenceWidth <= 0.01f) fenceWidth = 1f;

        int count = Mathf.Max(6, Mathf.RoundToInt(circumference / fenceWidth));

        spawned = new GameObject[count];

        float angleStep = 360f / count;

        for (int i = 0; i < count; i++)
        {
            float angle = i * angleStep * Mathf.Deg2Rad;

            float x = Mathf.Cos(angle) * radius;
            float z = Mathf.Sin(angle) * radius;

            Vector3 pos = center + new Vector3(x, 0f, z);

            GameObject obj = Instantiate(fencePrefab, pos, Quaternion.identity);

            Vector3 dir = (pos - center).normalized;

            Vector3 tangent = new Vector3(-dir.z, 0f, dir.x);

            obj.transform.rotation =
                Quaternion.LookRotation(tangent) *
                Quaternion.Euler(0f, rotationOffsetY, 0f);

            spawned[i] = obj;
        }
    }

    private float GetFenceWidth()
    {
        MeshFilter mf = fencePrefab.GetComponentInChildren<MeshFilter>();

        if (mf != null && mf.sharedMesh != null)
        {
            return mf.sharedMesh.bounds.size.z;
        }

        return 1f;
    }

    public void Clear()
    {
        if (spawned == null) return;

        foreach (var obj in spawned)
        {
            if (obj) Destroy(obj);
        }
    }
}