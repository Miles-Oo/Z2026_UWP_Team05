using UnityEngine;
using UnityEngine.InputSystem;

public class TowerSell : MonoBehaviour, IUseMode
{
    [SerializeField] private GameObject hammerAsset;
    [SerializeField] private Money money;
    [SerializeField] private CommandManager commandManager;

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

        Plane ground = new Plane(Vector3.up, Vector3.zero);

        if (!ground.Raycast(ray, out float dist))
            return;

        Vector3 pos = ray.GetPoint(dist);
        pos.y = 0f;

        if (Physics.Raycast(ray, out RaycastHit hit, 100f))
        {
            var tower = hit.collider.GetComponentInParent<TowerAttack>()?.gameObject;

            if (tower != null)
            {
                float snapDistance = 1.5f;

                if (Vector3.Distance(pos, tower.transform.position) <= snapDistance)
                {
                    pos = tower.transform.position;
                }
            }
        }

        if (preview != null)
            preview.transform.position = pos;
    }

    public bool ActionMode()
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();
        Ray ray = Camera.main.ScreenPointToRay(mousePos);

        if (Physics.Raycast(ray, out RaycastHit hit, 100f))
        {
            GameObject tower = hit.collider.GetComponentInParent<TowerRuntimeData>()?.gameObject;

            if (tower == null)
                return false;

            TowerPrice price = tower.GetComponent<TowerPrice>();
            ConstructionSide site = tower.GetComponentInParent<ConstructionSide>();

            var command = new CommandSell(
                tower,
                site,
                price.GetLevel(),
                price.GetPrice(),
                money
            );

            commandManager.ExecuteCommand(command);
                        Debug.Log("TOWER PREFAB FROM PRICE: " + price.GetTowerPrefab());
            ObserverSell.Instance.OnTowerSold(tower);

            ExitMode();
            return true;
        }

        return false;
    }
}