using System;
using UnityEngine;

public class ObserverUpgrade : MonoBehaviour
{
    public static ObserverUpgrade Instance;

    public event Action<GameObject> TowerUpgraded;

    private void Awake()
    {
        Instance = this;
    }

    public void OnTowerUpgraded(GameObject tower)
    {
        TowerUpgraded?.Invoke(tower);
    }
}