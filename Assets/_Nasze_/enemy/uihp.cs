using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class uihp : MonoBehaviour
{
    [SerializeField] private EnemyHp _enemyHp;
    [SerializeField] private Image _fill;
   // [SerializeField] private TextMeshProUGUI _text;

    private Color good;
    private Color bad;
    void Start(){
        _enemyHp.OnChangeHp += BarUpdate;
      
        good=new Color(0,255,0);
        bad=new Color(255,0,0);  
        BarUpdate();
    }
    void BarUpdate(){
        float precent=_enemyHp.GetCurrHp() / (float)_enemyHp.GetMaxHp();
        _fill.fillAmount = precent;

        //jeżeli głód spadnie poniżej 20% pasek głodu zmieni kolor na czerwony w innym na zielony
        _fill.color = precent < 0.2 ? bad : good;

    }
}
