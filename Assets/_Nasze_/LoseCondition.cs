using UnityEngine;
using UnityEngine.SceneManagement;

public class LoseCondition : MonoBehaviour
{
    [SerializeField] private HpModelBridge hpBridge;
    [SerializeField] private GameObject gameoverCanvas;

    private HpModel _model;

    void Start()
    {
        if (hpBridge == null)
        {
            Debug.LogError("Brak HpModelBridge!");
            return;
        }

        _model = hpBridge.GetModel();

        _model.OnHpChanged += CheckGameOver;

        if (gameoverCanvas != null)
            gameoverCanvas.SetActive(false);
    }

    void OnDestroy()
    {
        if (_model != null)
            _model.OnHpChanged -= CheckGameOver;
    }

    void CheckGameOver()
    {
        if (_model.CurrHp <= 0)
        {
            if (gameoverCanvas != null)
                gameoverCanvas.SetActive(true);

            if (GameManager.Instance != null)
            {
                GameManager.Instance.LoseGame();
            }
            else
            {
                Time.timeScale = 0f;
            }
        }
    }

    public void Retry()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.ReloadCurrentScene();
            return;
        }

        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
