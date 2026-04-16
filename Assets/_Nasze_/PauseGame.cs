using UnityEngine;
using UnityEngine.InputSystem;

public class PauseGame : MonoBehaviour
{
    private bool _isPaused=false;
    [SerializeField] GameObject pauseCanvas;
    private bool IsPaused => GameManager.Instance != null
        ? GameManager.Instance.CurrentState == GameState.Paused
        : _isPaused;
    void Start()
    {
        pauseCanvas.SetActive(false);
        _isPaused=false;
    }
    public void Pause(){
        if (GameManager.Instance != null)
        {
            GameManager.Instance.PauseGame();
        }
        else
        {
            Time.timeScale = 0f;
        }

        pauseCanvas.SetActive(true);
        _isPaused=true;
    }
    public void UnPause(){
        if (GameManager.Instance != null)
        {
            GameManager.Instance.ResumeGame();
        }
        else
        {
            Time.timeScale = 1f;
        }

        pauseCanvas.SetActive(false);
        _isPaused=false;
    }

void OnEnable()
{
    AllInputAction.escClickAction.Enable();
    AllInputAction.escClickAction.performed += OnEscClick;
}

void OnDisable()
{ 
    AllInputAction.escClickAction.Disable();
    AllInputAction.escClickAction.performed -= OnEscClick;
}
    
   
private void OnEscClick(InputAction.CallbackContext ctx)
{
    if (IsPaused){
        UnPause();
    }else{
        Pause();
    }
}
}
