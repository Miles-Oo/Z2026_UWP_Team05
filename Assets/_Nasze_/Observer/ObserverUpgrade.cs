using System;
using UnityEngine;

public class ObserverUpgrade : MonoBehaviour
{
    public static ObserverUpgrade Instance;

    public event Action<GameObject> TowerUpgraded;
    public event Action<GameObject> TowerStateReady;

    private void Awake()
    {
        Instance = this;
    }

    public void OnTowerUpgraded(GameObject tower)
    {
        TowerUpgraded?.Invoke(tower);
    }

    public void OnTowerStateReady(GameObject tower)
    {
        TowerStateReady?.Invoke(tower);
    }
}