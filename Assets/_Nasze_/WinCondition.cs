using System.Collections;
using UnityEngine;

public class WinCondition : MonoBehaviour
{
    [SerializeField] private WaveModel waveModel;
    [SerializeField] private GameObject winCanvasScreen;

    private void Start()
    {
        StartCoroutine(Initialize());
    }

    private IEnumerator Initialize()
    {
        // Czekamy jedna klatke, aby WaveManager zdazyl utworzyc/inicjalizowac model.
        yield return null;

        if (waveModel == null)
        {
            WaveManager waveManager = FindObjectOfType<WaveManager>();
            waveModel = waveManager != null ? waveManager.Model : null;
        }

        if (waveModel == null)
        {
            Debug.LogError("WaveModel nie jest przypisany!");
            yield break;
        }

        waveModel.OnEnemyChanged += CheckWin;

        if (winCanvasScreen != null)
        {
            winCanvasScreen.SetActive(false);
        }
    }

    private void CheckWin()
    {
        if (waveModel.CurrentWave == waveModel.TotalWaves &&
            waveModel.AliveEnemies <= 0)
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.WinGame();
            }
            else
            {
                Time.timeScale = 0f;
            }

            Debug.Log("Wygrales!");

            if (winCanvasScreen != null)
            {
                winCanvasScreen.SetActive(true);
            }
        }
    }

    private void OnDestroy()
    {
        if (waveModel != null)
        {
            waveModel.OnEnemyChanged -= CheckWin;
        }
    }
}
