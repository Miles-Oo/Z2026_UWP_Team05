using UnityEngine;
using TMPro;

public class playerUiCurrencies : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _textHp;
    [SerializeField] baseHp _baseHp;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _textHp.text="currhp/maxhp";        

        _baseHp.OnGetHp+= TextUpdate;
    }
    void TextUpdate()
    {
        _textHp.text=_baseHp.GetCurrHp()+"/"+_baseHp.GetMaxHp();
      
    }
}
