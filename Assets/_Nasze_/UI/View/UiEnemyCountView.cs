using UnityEngine;
using TMPro;

public class UiEnemyCountView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI enemyText;
    public void SetEnemies(int alive, int total)
    {
        if (alive == 0)
            enemyText.text = "All enemies destroyed";
        else
            enemyText.text = $"Enemies: {alive} / {total}";
    }
}