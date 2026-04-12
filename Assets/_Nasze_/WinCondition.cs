using UnityEngine;

public class WinCondition : MonoBehaviour
{
    [SerializeField] private WaveModel waveModel;
    [SerializeField] private GameObject winCanvasScreen;

    private void Start()
    {
        if (waveModel == null)
        {
            Debug.LogError("WaveModel nie jest przypisany!");
            return;
        }

        waveModel.OnEnemyChanged += CheckWin;

        winCanvasScreen.SetActive(false);
    }

    private void CheckWin()
    {
        if (waveModel.CurrentWave == waveModel.TotalWaves &&
            waveModel.AliveEnemies <= 0)
        {
            Time.timeScale = 0;
            Debug.Log("Wygrałeś!");
            winCanvasScreen.SetActive(true);
        }
    }

    private void OnDestroy()
    {
        waveModel.OnEnemyChanged -= CheckWin;
    }
}