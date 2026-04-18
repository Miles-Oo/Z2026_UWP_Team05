using UnityEngine;
using TMPro;

public class BaseHpView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textHp;

    public void SetText(int curr, int max)
    {
        textHp.text = $"Base HP: {curr}/{max}";
    }
}