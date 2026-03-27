using TMPro;
using UnityEngine;

public class TextSetterStart : MonoBehaviour
{
    private TextMeshProUGUI _text;
    [SerializeField] TowerPrice towerPrice;
    void Start()
    {
        _text=GetComponent<TextMeshProUGUI>();
        _text.text="Price: "+towerPrice.GetPrice();
    }
}
