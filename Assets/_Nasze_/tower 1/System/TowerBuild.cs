using UnityEngine;
using UnityEngine.InputSystem;

public class TowerBuild : MonoBehaviour, IUseMode
{
    [SerializeField] private TowerSelect towerSelect;
    [SerializeField] private TowerUpgrade towerUpgrade;
    [SerializeField] private GameObject towerPrefab;
    [SerializeField] private GameObject slowTowerPrefab;
    private GameObject selectedPrefab;
    [SerializeField] private Money money;
    [SerializeField] private LayerMask buildLayer;
    [SerializeField] private TutorialPopupController tutorialPopup;

    [Header("Command")]
    [SerializeField] private CommandManager commandManager;

    private int cost;
    private bool firstTowerPlaced = false;

    private GameObject preview;

    public Mode GetMode() => Mode.BUILD;

    void Start()
    {
        selectedPrefab = towerPrefab;
        cost = selectedPrefab.GetComponent<TowerPrice>().GetPrice();
    }

    public void SelectBasicTower()
    {
        selectedPrefab = towerPrefab;
        cost = selectedPrefab.GetComponent<TowerPrice>().GetPrice();

        RefreshPreview();
    }

    public void SelectSlowTower()
    {
        if (slowTowerPrefab == null)
        {
            return;
        }

        selectedPrefab = slowTowerPrefab;
        cost = selectedPrefab.GetComponent<TowerPrice>().GetPrice();

        RefreshPreview();
    }

    private void RefreshPreview()
    {
        if (preview != null)
            Destroy(preview);

        if (selectedPrefab == null) return;

        preview = Instantiate(selectedPrefab);
        preview.SetActive(true);

        var attack = preview.GetComponent<TowerAttack>();
        if (attack) attack.enabled = false;

        foreach (var col in preview.GetComponentsInChildren<Collider>())
            col.enabled = false;
    }

    public void EnterMode()
    {
        RefreshPreview();
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
        if (selectedPrefab == null)
        {
            Debug.Log("Nie wybrano wieży!");
            return false;
        }
        
        Vector2 mousePos = Mouse.current.position.ReadValue();
        Ray ray = Camera.main.ScreenPointToRay(mousePos);

        if (Physics.Raycast(ray, out RaycastHit hit, 100f, buildLayer))
        {
            ConstructionSide site = hit.collider.GetComponentInParent<ConstructionSide>();

            if (site != null && site.IsFree())
            {
                TowerType type = GetSelectedType();
                Debug.Log($"[TOWER BUILD] Selected type = {type}");

                GameObject prefab = TowerFactory.GetPrefab(type);

                if (prefab == null)
                {
                    Debug.Log("Factory nie zwróciła prefabu!");
                    return false;
                }

                int currentCost = prefab.GetComponent<TowerPrice>().GetPrice();
                if (money.GetCurrMoney() >= currentCost)
                {
                    var command = new CommandBuild(
                        prefab,
                        site,
                        money,
                        cost,
                        towerSelect,
                        towerUpgrade
                    );

                    commandManager.ExecuteCommand(command);
                    ObserverBuild.Instance.OnTowerBuilt(site.gameObject);

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

    private TowerType GetSelectedType()
    {
        if (selectedPrefab == towerPrefab)
            return TowerType.Basic;

        if (selectedPrefab == slowTowerPrefab)
            return TowerType.Slow;

        return TowerType.Basic;
    }
}