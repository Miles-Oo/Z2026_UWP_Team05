using System;
using UnityEngine;

public class ObserverBuild : MonoBehaviour
{
    public static ObserverBuild Instance;

    public event Action<GameObject> TowerBuilt;

    private void Awake()
    {
        Instance = this;
    }

    public void OnTowerBuilt(GameObject tower)
    {
        TowerBuilt?.Invoke(tower);
    }
}