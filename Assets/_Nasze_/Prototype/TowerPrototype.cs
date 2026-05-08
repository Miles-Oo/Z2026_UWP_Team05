using UnityEngine;

public class TowerPrototype : MonoBehaviour, ITowerPrototype
{
    public GameObject Clone(Vector3 position)
    {
        Debug.Log("KLONOWANIE PROTOTYPU: " + gameObject.name);
        GameObject clone = Instantiate(
            gameObject,
            position,
            Quaternion.identity
        );

        clone.name = gameObject.name + "_Clone";

        return clone;
    }
}