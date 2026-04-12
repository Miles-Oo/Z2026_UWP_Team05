using UnityEngine;
using UnityEngine.InputSystem;

public class PauseGame : MonoBehaviour
{
    private bool _isPaused=false;
    [SerializeField] GameObject pauseCanvas;
    void Start()
    {
        pauseCanvas.SetActive(false);
        _isPaused=false;
    }
    public void Pause(){
        Time.timeScale=0;
        pauseCanvas.SetActive(true);
        _isPaused=true;
    }
    public void UnPause(){
        Time.timeScale=1;
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
    if(_isPaused){
        UnPause();
    }else{
        Pause();
    }
}
}
