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
    [SerializeField] private TowerUpgradeUI upgradeButton;

    [Header("Strategy UI")]
    [SerializeField] private TowerTargetStrategyUI strategyUI;

    [Header("Upgrade System")]
    [SerializeField] private TowerUpgrade towerUpgrade;

    [Header("Tutorial")]
    [SerializeField] private TutorialPopupController tutorialPopup;

    private GameObject selectedTower;
    private bool firstTowerClicked = false;

    private TowerPresenter presenter;
    [SerializeField] private CommandManager commandManager;

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

        if (!Physics.Raycast(ray, out RaycastHit hit, 100f, towerLayer, QueryTriggerInteraction.Ignore))
        {
            ClearSelection();
            return false;
        }

        GameObject tower = null;

        var basic = hit.collider.GetComponentInParent<TowerAttack>();
        if (basic != null)
        {
            tower = basic.gameObject;
        }
        else
        {
            var slow = hit.collider.GetComponentInParent<SlowTowerController>();

            if (slow != null)
                tower = slow.gameObject;
        }

        if (tower == null)
        {
            ClearSelection();
            return false;
        }

        selectedTower = tower;
        towerUpgrade.SetSelectedTower(tower);

        var attack = selectedTower.GetComponent<TowerAttack>();

        if (attack != null)
        {
            rangeVisualizer.ShowRange(
                selectedTower.transform.position,
                attack.GetRange()
            );
        }
        else
        {
            var slow = selectedTower.GetComponent<SlowTowerController>();

            if (slow != null)
            {
                rangeVisualizer.ShowRange(
                    selectedTower.transform.position,
                    slow.GetRange()
                );
            }
            else
            {
                ClearSelection();
                return false;
            }
        }

        towerInfoPanel.SetActive(true);

        var model = new TowerModel(tower);

        if (presenter != null)
        {
            presenter.Remove();
        }
        presenter = new TowerPresenter(model, upgradeButton, towerUpgrade, commandManager);

        if (strategyUI != null)
        {
            if (attack != null)
                strategyUI.SetTower(attack);
            else
            {
                var slow = selectedTower.GetComponent<SlowTowerController>();

                if (slow != null)
                    strategyUI.SetSlowTower(slow);
            }
        }
        if (!firstTowerClicked && tutorialPopup != null)
        {
            tutorialPopup.ShowTowerStrategyPopup();
            firstTowerClicked = true;
        }

        return false;
    }

    private void ClearSelection()
    {
        rangeVisualizer.ClearSelectionRange(selectedTower);
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
    public void ForceClearSelection()
    {
        ClearSelection();

        if (towerUpgrade != null)
            towerUpgrade.ClearSelection(); 
    }
}