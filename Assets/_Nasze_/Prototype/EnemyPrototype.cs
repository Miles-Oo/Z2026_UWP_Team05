using UnityEngine;

public class EnemyPrototype : MonoBehaviour, IEnemyPrototype
{
    public GameObject Clone(Vector3 position)
    {
        GameObject clone = Instantiate(gameObject, position, Quaternion.identity);

        Debug.Log("KLONOWANIE PROTOTYPU ENEMY: " + gameObject.name);

        return clone;
    }
}