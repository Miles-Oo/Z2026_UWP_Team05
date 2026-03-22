using UnityEngine;
using TMPro;

public class UiWaveEnemy : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _textHp;
    [SerializeField] Money _money;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _textHp.text="currhp/maxhp";        

        _money.OnGetMoney+= TextUpdate;
        TextUpdate();
    }
    void TextUpdate()
    {
        _textHp.text="Money: "+_money.GetCurrMoney()+"";
      
    }
}
