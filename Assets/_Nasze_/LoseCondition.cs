using UnityEngine;
using UnityEngine.SceneManagement;

public class LoseCondition : MonoBehaviour
{
    [SerializeField] baseHp _baseHp;
    [SerializeField] GameObject gameoverCanvas;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _baseHp.OnHpChanged+=Gameover;
        gameoverCanvas.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void Gameover(){
        if(_baseHp.GetCurrHp()<=0){
            gameoverCanvas.SetActive(true);
            Time.timeScale=0;
        }
    }
   public void Retry(){
              Time.timeScale=1;
                 string curScName=SceneManager.GetActiveScene().name;
              SceneManager.LoadScene(curScName);
    }
}
