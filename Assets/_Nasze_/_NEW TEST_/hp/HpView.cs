using UnityEngine;
using TMPro;

public class HpView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _textHp;

    public void SetHp(int curr, int max)
    {
        _textHp.text = $"HP: {curr}/{max}";
    }
}