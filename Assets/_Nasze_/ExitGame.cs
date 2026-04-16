using UnityEngine;

public class ExitGame : MonoBehaviour
{

    public void ExitGameFF(){
        if (GameManager.Instance != null)
        {
            GameManager.Instance.QuitGame();
            return;
        }

        Application.Quit();
    }
}
