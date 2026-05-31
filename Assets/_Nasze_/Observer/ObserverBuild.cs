using System;
using UnityEngine;

public class ObserverBuild : MonoBehaviour
{
    public static ObserverBuild Instance;

    public event Action<GameObject> TowerBuilt;

    private void Awake()
    {
        Instance = this;
        TowerBuilt += PlayBuildSfx;
    }

    private void OnDestroy()
    {
        TowerBuilt -= PlayBuildSfx;
    }

    public void OnTowerBuilt(GameObject tower)
    {
        TowerBuilt?.Invoke(tower);
    }

    private void PlayBuildSfx(GameObject tower)
    {
        Debug.Log("TowerBuild SFX");
        AudioManager.Instance?.PlaySfx(AudioCue.TowerBuild);
    }
}