using UnityEngine;
using UnityEngine.InputSystem;

public class TowerBuild : MonoBehaviour, IUseMode
{
    [SerializeField] private GameObject towerPrefab;
    [SerializeField] private Money money;
    [SerializeField] private LayerMask buildLayer;
    [SerializeField] private TutorialPopupController tutorialPopup;

    private int cost;
    private bool firstTowerPlaced = false;

    private GameObject preview;

    public Mode GetMode() => Mode.BUILD;

    void Start()
    {
        cost = towerPrefab.GetComponent<TowerPrice>().GetPrice();
    }

    public void EnterMode()
    {
        preview = Instantiate(towerPrefab);
        preview.SetActive(true);

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

        Plane plane = new Plane(Vector3.up, Vector3.zero);
        if (!plane.Raycast(ray, out float dist)) return;

        Vector3 pos = ray.GetPoint(dist);
        pos.y = 0f;

        if (Physics.Raycast(ray, out RaycastHit hit, 100f, buildLayer))
        {
            var site = hit.collider.GetComponentInParent<ConstructionSide>();
            if (site != null && site.IsFree())
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

        if (Physics.Raycast(ray, out RaycastHit hit, 100f, buildLayer))
        {
            ConstructionSide site = hit.collider.GetComponentInParent<ConstructionSide>();

            if (site != null && site.IsFree())
            {
                if (money.GetCurrMoney() >= cost)
                {
                    money.SubMoney(cost);

                    GameObject tower = Instantiate(towerPrefab, site.transform.position, Quaternion.identity);
                    tower.transform.SetParent(site.transform);
                    tower.transform.localPosition = Vector3.zero;
                    site.SetTower(tower);

                    if (!firstTowerPlaced && tutorialPopup != null)
                    {
                        tutorialPopup.ShowTowerUpgradePopup();
                        firstTowerPlaced = true;
                    }

                    return true;
                }
                else
                {
                    Debug.Log("Nie masz wystarczająco pieniędzy!");
                }
            }
        }
        return false;
    }
}