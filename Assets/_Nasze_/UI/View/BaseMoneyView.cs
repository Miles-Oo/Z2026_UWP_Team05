using UnityEngine;
using TMPro;

public class BaseMoneyView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textHp;

    public void SetText(int curr)
    {
        textHp.text = $"Money: {curr}";
    }
}