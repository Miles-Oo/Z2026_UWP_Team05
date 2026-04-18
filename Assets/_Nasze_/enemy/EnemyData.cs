using UnityEngine;

public class EnemyData : MonoBehaviour
{
    [SerializeField] private string enemyName;

    public string Name => enemyName;
}