using UnityEngine;

public class PauseInputHandler : MonoBehaviour
{
    private void OnEnable()
    {
        AllInputAction.Enable();
    }

    private void OnDisable()
    {
        AllInputAction.Disable();
    }

    void Update()
    {
        if (AllInputAction.escClickAction.WasPressedThisFrame())
        {
            TogglePause();
        }
    }

    private void TogglePause()
    {
        if (GameManager.Instance.CurrentState == GameState.Playing)
        {
            GameManager.Instance.PauseGame();
        }
        else if (GameManager.Instance.CurrentState == GameState.Paused)
        {
            GameManager.Instance.ResumeGame();
        }
    }
}