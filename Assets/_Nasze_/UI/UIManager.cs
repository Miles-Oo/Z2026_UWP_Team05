using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private SlowTowerHoverInfo hoverInfo;

    public void ShowTowerStats(SlowTowerStats stats)
    {
        hoverInfo.Show(stats);
    }

    public void HideTowerStats()
    {
        hoverInfo.Hide();
    }
}