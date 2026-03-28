using UnityEngine;
using UnityEngine.InputSystem;

public class TowerSell : MonoBehaviour, IUseMode
{
    [SerializeField] private GameObject hammerAsset;
    [SerializeField] private Money money;
    [SerializeField] private LayerMask buildLayer;

    private GameObject preview;

    public Mode GetMode() => Mode.SELL;


    public void EnterMode() { preview = Instantiate(hammerAsset);}
    public void ExitMode() { if (preview) Destroy(preview); }

    public void PrewMode()
    {    Vector2 mousePos = Mouse.current.position.ReadValue();
    Ray ray = Camera.main.ScreenPointToRay(mousePos);

    Vector3 pos;

    Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
    if (groundPlane.Raycast(ray, out float distance))
    {
        pos = ray.GetPoint(distance);
        pos.y = 0f;
    }
    else
    {
        return;
    }

   
    if (Physics.Raycast(ray, out RaycastHit hit,100,buildLayer))
    {
        ConstructionSide site = hit.collider.GetComponent<ConstructionSide>();

        if (site != null && !site.IsFree())
        {
          
            pos = hit.collider.transform.position;
        }
    }

    preview.transform.position = pos;
    }

    public bool ActionMode()
    {
    Vector2 mousePos = Mouse.current.position.ReadValue();
    Ray ray = Camera.main.ScreenPointToRay(mousePos);

    if (Physics.Raycast(ray, out RaycastHit hit,100,buildLayer))
    {
        ConstructionSide site = hit.collider.GetComponent<ConstructionSide>();

        if (site != null && !site.IsFree()){
              money.AddMoney(site.GetPlacedTower().GetComponent<TowerPrice>().GetPrice()/2);
              site.SetTower(null);
              ExitMode();
              return true;
        }
    }

    return false;
    }
}