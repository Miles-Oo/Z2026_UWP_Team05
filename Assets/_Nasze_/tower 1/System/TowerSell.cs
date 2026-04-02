using UnityEngine;
using UnityEngine.InputSystem;

public class TowerSell : MonoBehaviour, IUseMode
{
    [SerializeField] private GameObject hammerAsset;
    [SerializeField] private Money money;
    [SerializeField] private LayerMask buildLayer;

    private GameObject preview;

    public Mode GetMode() => Mode.SELL;

    public void EnterMode()
    {
        preview = Instantiate(hammerAsset);
    }

    public void ExitMode()
    {
        if (preview) Destroy(preview);
    }

    public void PrewMode()
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();
        Ray ray = Camera.main.ScreenPointToRay(mousePos);

        Vector3 pos;

        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        if (groundPlane.Raycast(ray, out float distance))
        {
            pos = ray.GetPoint(distance);
            pos.y = 0f;
        }
        else return;

        if (Physics.Raycast(ray, out RaycastHit hit, 100f))
        {
            var site = hit.collider.GetComponentInParent<ConstructionSide>();

            if (site != null && !site.IsFree())
            {
                pos = site.transform.position;
            }
        }

        preview.transform.position = pos;
    }

    public bool ActionMode()
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();
        Ray ray = Camera.main.ScreenPointToRay(mousePos);

        if (!Physics.Raycast(ray, out RaycastHit hit, 100f))
            return false;

        GameObject clicked = hit.collider.gameObject;

        var tower = clicked.GetComponentInParent<TowerAttack>()?.gameObject;

        if (tower == null)
            return false;

        var site = tower.GetComponentInParent<ConstructionSide>();

        var price = tower.GetComponent<TowerPrice>();
        if (price != null)
        {
            money.AddMoney(price.GetPrice() / 2);
        }

        Destroy(tower);

        if (site != null)
        {
            site.SetTower(null);
        }

        ExitMode();
        return true;
    }
}