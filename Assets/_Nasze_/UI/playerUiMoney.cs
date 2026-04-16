using UnityEngine;
using TMPro;

public class playerUiMoney : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _textHp;
    [SerializeField] Money _money;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _textHp.text="currhp/maxhp";        

        _money.OnGetMoney += UpdateMoneyText;
        UpdateMoneyText();
    }

    void OnDestroy()
    {
        if (_money != null)
        {
            _money.OnGetMoney -= UpdateMoneyText;
        }
    }

    private void UpdateMoneyText()
    {
        _textHp.text="Gold: "+_money.GetCurrMoney()+"";
      
    }
}
