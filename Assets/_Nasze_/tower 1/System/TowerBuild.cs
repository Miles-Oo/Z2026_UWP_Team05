using UnityEngine;
using UnityEngine.InputSystem;

public class TowerBuild : MonoBehaviour, IUseMode
{
    [SerializeField] private GameObject towerPrefab;
    [SerializeField] private Money money;
    [SerializeField] private LayerMask buildLayer;

    private GameObject preview;
    private int cost;

    public Mode GetMode() => Mode.BUILD;

    void Start()
    {
        cost = towerPrefab.GetComponent<TowerPrice>().GetPrice();
    }

    public void EnterMode()
    {
        preview = Instantiate(towerPrefab);

        // wyłącz logikę TYLKO na instancji
        var attack = preview.GetComponent<TowerAttack>();
        if (attack) attack.enabled = false;

        foreach (var col in preview.GetComponentsInChildren<Collider>())
            col.enabled = false;
    }

    public void ExitMode()
    {
        if (preview) Destroy(preview);
    }

    public void PrewMode()
    {
        if (!preview) return;

        Vector2 mousePos = Mouse.current.position.ReadValue();
        Ray ray = Camera.main.ScreenPointToRay(mousePos);

        Vector3 pos;

        Plane plane = new Plane(Vector3.up, Vector3.zero);
        if (!plane.Raycast(ray, out float dist)) return;

        pos = ray.GetPoint(dist);
        pos.y = 0f;

        if (Physics.Raycast(ray, out RaycastHit hit, 100f, buildLayer))
        {
            var site = hit.collider.GetComponent<ConstructionSide>();
            if (site != null && site.IsFree())
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

        if (site != null && site.IsFree())
        {
            if (money.GetCurrMoney() >= cost)
            {
                money.SubMoney(cost);

               GameObject g= Instantiate(towerPrefab, hit.collider.transform.position, Quaternion.identity);
                site.SetTower(g);
                Debug.Log("Wieża postawiona!");
                return true;
            }
            else
            {
                Debug.Log("Nie masz wystarczająco pieniędzy!");
                return false;
            }
        }
    }
    return false;
    }
    
}