using UnityEngine;
using UnityEngine.InputSystem;

public class TowerSelect : MonoBehaviour, IUseMode
{
    [SerializeField] private LayerMask towerLayer;
    [SerializeField] private RangeVisualizer rangeVisualizer;

    private GameObject selected;

    public Mode GetMode() => Mode.SELECT;

    public void EnterMode() { }

    public void ExitMode()
    {
        selected = null;
        rangeVisualizer.Clear();
    }

    public void PrewMode() { }

    public bool ActionMode()
    {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (Physics.Raycast(ray, out RaycastHit hit, 100f, towerLayer))
        {
            GameObject tower = hit.collider.gameObject;

            var attack = tower.GetComponent<TowerAttack>();
            var price = tower.GetComponent<TowerPrice>();

            if (attack != null && price != null)
            {
                selected = tower;

                float range = tower.GetComponent<SphereCollider>().radius * tower.transform.localScale.x;

                rangeVisualizer.ShowRange(
                    tower.transform.position,
                    range
                );

                Debug.Log("DMG: " + attack.GetDamage());
                Debug.Log("Sell: " + price.GetPrice() / 2);

                return false;
            }
        }

        selected = null;
        rangeVisualizer.Clear();

        return false;
    }
}