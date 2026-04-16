using UnityEngine;
using TMPro;

public class WaveView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _waveText;
    [SerializeField] private TextMeshProUGUI _enemyText;

    public void SetWave(int current, int total)
    {
        _waveText.text = $"Wave: {current}/{total}";
    }

    public void SetEnemies(int alive, int total)
    {
        if (alive == 0)
            _enemyText.text = "All enemies destroyed";
        else
            _enemyText.text = $"Enemies: {alive}/{total}";
    }
}