using UnityEngine;

public class GameAssets : MonoBehaviour
{
    public static GameAssets Instance;

    public GameObject basicTowerPrefab;
    public GameObject slowTowerPrefab;

    public GameObject grazynaEnemyPrefab;
    public GameObject jolaEnemyPrefab;

    private void Awake()
    {
        Instance = this;
    }
}