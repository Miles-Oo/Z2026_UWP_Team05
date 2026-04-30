using UnityEngine;

public class GameAudioObserver : MonoBehaviour
{
    private void OnEnable()
    {
        GameManager.Instance.OnStateChanged += HandleStateChange;
    }

    private void OnDisable()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.OnStateChanged -= HandleStateChange;
    }

    private void HandleStateChange(GameState state)
    {
        switch (state)
        {
            case GameState.Paused:
                AudioManager.Instance?.PlaySfx(AudioCue.Pause);
                AudioManager.Instance?.PauseMusic();
                break;

            case GameState.Playing:
                AudioManager.Instance?.PlaySfx(AudioCue.Resume);
                AudioManager.Instance?.ResumeMusic();
                break;

            case GameState.Won:
                AudioManager.Instance?.PlaySfx(AudioCue.Win);
                break;

            case GameState.Lost:
                AudioManager.Instance?.PlaySfx(AudioCue.Lose);
                break;
        }
    }
}