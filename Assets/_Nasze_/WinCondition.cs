using UnityEngine;

public class WinCondition : MonoBehaviour
{
    [SerializeField] WaveManager waveManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        waveManager.OnEnemyCountChanged+=GameWin;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void GameWin(){
        if(waveManager.aliveEnemies<=0){
              Debug.Log("Wygrałeś");
        }
    }
}
