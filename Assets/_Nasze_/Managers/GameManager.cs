using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState
{
    Playing,
    Paused,
    Won,
    Lost
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private bool persistBetweenScenes = true;
    public GameState CurrentState { get; private set; } = GameState.Playing;

    public event Action<GameState> OnStateChanged;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Bootstrap()
    {
        if (Instance != null)
        {
            return;
        }

        GameObject managerObject = new GameObject(nameof(GameManager));
        managerObject.AddComponent<GameManager>();
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        if (persistBetweenScenes)
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    public void PauseGame()
    {
        if (CurrentState != GameState.Playing)
        {
            return;
        }

        SetState(GameState.Paused);
        AudioManager.Instance?.PlaySfx(AudioCue.Pause);
    }

    public void ResumeGame()
    {
        if (CurrentState != GameState.Paused)
        {
            return;
        }

        SetState(GameState.Playing);
        AudioManager.Instance?.PlaySfx(AudioCue.Resume);
    }

    public void WinGame()
    {
        if (CurrentState == GameState.Won)
        {
            return;
        }

        SetState(GameState.Won);
        AudioManager.Instance?.PlaySfx(AudioCue.Win);
    }

    public void LoseGame()
    {
        if (CurrentState == GameState.Lost)
        {
            return;
        }

        SetState(GameState.Lost);
        AudioManager.Instance?.PlaySfx(AudioCue.Lose);
    }

    public void LoadScene(string sceneName)
    {
        if (string.IsNullOrWhiteSpace(sceneName))
        {
            Debug.LogWarning("Scene name is empty.");
            return;
        }

        ResetToGameplayState();
        SceneManager.LoadScene(sceneName);
    }

    public void ReloadCurrentScene()
    {
        ResetToGameplayState();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private void ResetToGameplayState()
    {
        SetState(GameState.Playing);
    }

    private void SetState(GameState newState)
    {
        CurrentState = newState;
        Time.timeScale = newState == GameState.Playing ? 1f : 0f;
        OnStateChanged?.Invoke(newState);
    }
}
