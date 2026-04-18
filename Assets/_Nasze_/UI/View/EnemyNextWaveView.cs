using TMPro;
using UnityEngine;

public class EnemyNextWaveView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI totalText;
    [SerializeField] private TextMeshProUGUI typesText;

    public void SetText(string total, string types)
    {
        totalText.text = total;
        typesText.text = types;
    }
}