using UnityEngine;
using TMPro;

public class MoneyView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;

    public void SetMoney(int value)
    {
        _text.text = $"Gold: {value}";
    }
}