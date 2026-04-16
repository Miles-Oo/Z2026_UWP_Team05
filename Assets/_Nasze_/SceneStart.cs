using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneStart : MonoBehaviour
{
    [SerializeField] public string scenaGry;

    public void RunThatScene(){
        if (GameManager.Instance != null)
        {
            GameManager.Instance.LoadScene(scenaGry);
            return;
        }

        SceneManager.LoadScene(scenaGry);
    }
}
