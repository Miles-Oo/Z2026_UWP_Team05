using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class TowerSelect : MonoBehaviour, IUseMode
{
    [Header("Layers & Visuals")]
    [SerializeField] private LayerMask towerLayer;
    [SerializeField] private RangeVisualizer rangeVisualizer;

    [Header("UI")]
    [SerializeField] private GameObject towerInfoPanel;
    [SerializeField] private TowerUpgradeUI upgradeUI;

    [Header("Upgrade System")]
    [SerializeField] private TowerUpgrade towerUpgrade;

    [Header("Tutorial")]
    [SerializeField] private TutorialPopupController tutorialPopup;

    private GameObject selectedTower;
    private bool firstTowerClicked = false;

    private TowerPresenter presenter;

    public Mode GetMode() => Mode.SELECT;

    public void EnterMode() { }

    public void ExitMode()
    {
        ClearSelection();
    }

    public void PrewMode() { }

    public void SetSelectedTower(GameObject tower)
    {
        selectedTower = tower;
    }

    public bool ActionMode()
    {
        if (IsPointerOverUI()) return false;

        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (!Physics.Raycast(ray, out RaycastHit hit, 100f))
        {
            ClearSelection();
            return false;
        }

        GameObject tower = hit.collider.GetComponentInParent<TowerAttack>()?.gameObject;

        if (tower == null)
        {
            ClearSelection();
            return false;
        }

        selectedTower = tower;
        towerUpgrade.SetSelectedTower(tower);

        var attack = selectedTower.GetComponent<TowerAttack>();
        if (attack == null)
        {
            ClearSelection();
            return false;
        }

        rangeVisualizer.ShowRange(selectedTower.transform.position, attack.GetRange());

        towerInfoPanel.SetActive(true);

        var model = new TowerModel(tower);

        if (presenter != null)
        {
            presenter.Remove();
        }
        presenter = new TowerPresenter(model, upgradeUI, towerUpgrade);

        if (!firstTowerClicked && tutorialPopup != null)
        {
            tutorialPopup.ShowTowerStrategyPopup();
            firstTowerClicked = true;
        }

        return false;
    }

    private void ClearSelection()
    {
        rangeVisualizer.Clear();
        towerInfoPanel.SetActive(false);

        if (presenter != null)
        {
            presenter.Remove();
        }
        presenter = null;

        selectedTower = null;
    }

    private bool IsPointerOverUI()
    {
        if (EventSystem.current == null) return false;

        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Mouse.current.position.ReadValue();

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        return results.Count > 0;
    }
}