using UnityEngine;
public interface IEnemyPrototype
{
    GameObject Clone(Vector3 position);
}