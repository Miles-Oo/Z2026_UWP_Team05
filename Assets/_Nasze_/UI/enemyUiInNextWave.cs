using UnityEngine;
using TMPro;
public class enemyUiInNextWave : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _waveText;
    [SerializeField] private TextMeshProUGUI _enemyText;
    [SerializeField] private WaveManager _waveManager;
    // Update is called once per frame
     private void Start()
    {
        if (_waveManager == null)
        {
            Debug.LogError("WaveManager nie jest ustawiony w UI!");
            return;
        }
        _waveManager.OnEnemyCountChanged +=UpdateWaveText;
        _waveManager.OnEnemyCountChanged += UpdateEnemyText;

        UpdateEnemyText();
        UpdateWaveText();
    }


    private void UpdateEnemyText()
    {

       int totalEnemiesInWave = 0;
            foreach (var entry in _waveManager.waves[_waveManager.currentWaveNumber+1].enemies)
            {
                totalEnemiesInWave += entry.count;
            }
        _enemyText.text = $"Enemies: {totalEnemiesInWave}";
    }
    private void UpdateWaveText(){
        string buildstring="";
        for(int i =0;i<_waveManager.waves[_waveManager.currentWaveNumber+1].enemies.Length;i++){
      buildstring+= _waveManager.waves[_waveManager.currentWaveNumber+1].enemies[i].enemyName+" ";
        }
        _waveText.text=buildstring;
    }
}
