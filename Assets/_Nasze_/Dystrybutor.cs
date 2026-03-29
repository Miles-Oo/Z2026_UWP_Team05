using UnityEngine;

public class DistributeObjects : MonoBehaviour
{
    public Transform[] objects;
    public float spacing = 2f;
    public Vector3 direction = Vector3.right;

    [ContextMenu("Distribute")]
    void Distribute()
    {
        if (objects == null || objects.Length == 0) return;

        Vector3 startPos = objects[0].position;

        for (int i = 0; i < objects.Length; i++)
        {
            objects[i].position = startPos + direction.normalized * spacing * i;
        }
    }
}