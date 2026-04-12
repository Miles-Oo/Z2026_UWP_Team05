using UnityEngine;

public class WinCondition : MonoBehaviour
{
    [SerializeField] WaveManager waveManager;
    [SerializeField] GameObject winCanvasScreen;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        waveManager.OnEnemyCountChanged+=GameWin;
        winCanvasScreen.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void GameWin(){
        if(waveManager.currentWaveNumber==waveManager.TotalWaves){
             if(waveManager.aliveEnemies<=0){
             Time.timeScale=0;
              Debug.Log("Wygrałeś");
              winCanvasScreen.SetActive(true);
        }
        }
       
    }
}
