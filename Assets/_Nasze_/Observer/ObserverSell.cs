using System;
using UnityEngine;

public class ObserverSell : MonoBehaviour
{
    public static ObserverSell Instance;

    public event Action<GameObject> TowerSold;

    private void Awake()
    {
        Instance = this;
    }

    public void OnTowerSold(GameObject tower)
    {
        TowerSold?.Invoke(tower);
    }
}