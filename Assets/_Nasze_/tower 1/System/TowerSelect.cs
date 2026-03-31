using UnityEngine;
using UnityEngine.InputSystem;

public class TowerSelect : MonoBehaviour, IUseMode
{
    [SerializeField] private LayerMask towerLayer;
    [SerializeField] private RangeVisualizer rangeVisualizer;

    public Mode GetMode() => Mode.SELECT;

    public void EnterMode() { }

    public void ExitMode()
    {
        rangeVisualizer.Clear();
    }

    public void PrewMode() { }

public bool ActionMode()
{
    Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

    if (!Physics.Raycast(ray, out RaycastHit hit, 100f, towerLayer))
    {
        rangeVisualizer.Clear();
        return false;
    }

    var site = hit.collider.GetComponent<ConstructionSide>();

    if (site == null || site.IsFree())
    {
        rangeVisualizer.Clear();
        return false;
    }

    var tower = site.GetPlacedTower();
    var attack = tower.GetComponent<TowerAttack>();
    var price  = tower.GetComponent<TowerPrice>();

    rangeVisualizer.ShowRange(
        tower.transform.position,
        attack.GetRange()
    );

    Debug.Log($"DMG: {attack.GetDamage()}");
    Debug.Log($"Sell: {price.GetPrice() / 2}");

    return false;
}
}