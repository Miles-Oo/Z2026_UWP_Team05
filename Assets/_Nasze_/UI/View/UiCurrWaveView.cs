using UnityEngine;
using TMPro;

public class UiCurrWaveView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI waveText;
    public void SetWave(int current, int total)
    {
        waveText.text = $"Wave: {current} / {total}";
    }
}