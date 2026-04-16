using UnityEngine;
using UnityEngine.InputSystem;

public class TowerSell : MonoBehaviour, IUseMode
{
    [SerializeField] private GameObject hammerAsset;
    [SerializeField] private Money money;
    [SerializeField] private MoneyBridge moneyBridge;
    [SerializeField] private LayerMask buildLayer;
    [SerializeField] private TutorialPopupController tutorialPopup;

    private GameObject preview;
    private MoneyModel _moneyModel;

    public Mode GetMode() => Mode.SELL;

    public void EnterMode()
    {
        preview = Instantiate(hammerAsset);

        if (tutorialPopup == null)
        {
            tutorialPopup = FindObjectOfType<TutorialPopupController>();
        }

        tutorialPopup?.ShowTowerSellPopup();
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
        var tower = hit.collider.GetComponentInParent<TowerAttack>()?.gameObject;
        if (tower != null)
            {
                Vector3 cursorPos = pos;
                float snapDistance = 1.5f;

                if (Vector3.Distance(cursorPos, tower.transform.position) <= snapDistance)
                {
                    pos = tower.transform.position;
                }
            }
        }

        preview.transform.position = pos;
    }

    public bool ActionMode()
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();
        Ray ray = Camera.main.ScreenPointToRay(mousePos);

        if (Physics.Raycast(ray, out RaycastHit hit, 100))
        {
            GameObject tower = hit.collider.GetComponentInParent<TowerAttack>()?.gameObject;

            if (tower != null)
            {
                ConstructionSide site = tower.GetComponentInParent<ConstructionSide>();
                TowerPrice price = tower.GetComponent<TowerPrice>();

                if (price != null)
                {
                    if (!TryAddMoney(price.GetPrice()))
                    {
                        Debug.LogError("TowerSell: no money source found (Money or MoneyBridge).");
                        return false;
                    }
                }

                if (site != null)
                {
                    site.SetTower(null);
                }

                Destroy(tower);
                ExitMode();
                return true;
            }
        }

        return false;
    }

    private bool TryAddMoney(int amount)
    {
        if (money != null)
        {
            money.AddMoney(amount);
            return true;
        }

        if (TryResolveMoneyModel())
        {
            _moneyModel.AddMoney(amount);
            return true;
        }

        return false;
    }

    private bool TryResolveMoneyModel()
    {
        if (_moneyModel != null)
        {
            return true;
        }

        if (moneyBridge == null)
        {
            moneyBridge = FindObjectOfType<MoneyBridge>();
            if (moneyBridge == null)
            {
                return false;
            }
        }

        _moneyModel = moneyBridge.GetModel();
        return _moneyModel != null;
    }
}
