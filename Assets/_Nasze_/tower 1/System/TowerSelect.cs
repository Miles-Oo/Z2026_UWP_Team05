using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class TowerSelect : MonoBehaviour, IUseMode
{
    [Header("Layers & Visuals")]
    [SerializeField] private LayerMask towerLayer;
    [SerializeField] private RangeVisualizer rangeVisualizer;

    [Header("UI")]
    [SerializeField] private GameObject towerInfoPanel;
    [SerializeField] private GameObject upgradeButton;

    [Header("Upgrade System")]
    [SerializeField] private TowerUpgrade towerUpgrade;

    [Header("Tutorial")]
    [SerializeField] private TutorialPopupController tutorialPopup;

    private GameObject selectedTower;
    private bool tutorialShown = false; // 🔹 sprawdzamy, czy popup został już pokazany

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

        if (!Physics.Raycast(ray, out RaycastHit hit, 100f, towerLayer))
        {
            ClearSelection();
            return false;
        }

        var site = hit.collider.GetComponent<ConstructionSide>();
        if (site == null || site.IsFree())
        {
            ClearSelection();
            return false;
        }

        GameObject tower = site.GetPlacedTower();
        if (tower == null)
        {
            ClearSelection();
            return false;
        }

        selectedTower = tower;

        var attack = selectedTower.GetComponent<TowerAttack>();
        var price = selectedTower.GetComponent<TowerPrice>();

        if (attack == null || price == null)
        {
            ClearSelection();
            return false;
        }

        // 🔹 rysowanie zasięgu w trybie select
        rangeVisualizer.ShowRange(selectedTower.transform.position, attack.GetRange());

        // 🔹 włączamy panel info o wieży
        towerInfoPanel.SetActive(true);

        // 🔹 dynamiczne podpięcie przycisku Upgrade
        var btn = upgradeButton.GetComponent<Button>();
        btn.onClick.RemoveAllListeners();
        btn.onClick.AddListener(() => towerUpgrade.SetSelectedTower(selectedTower));

        // 🔹 pokazanie popupu tutorialowego przy pierwszym kliknięciu
        if (!tutorialShown && tutorialPopup != null)
        {
            tutorialPopup.ShowTowerUpgradePopup(); // 🔹 pokazuje nowy popup przy kliknięciu wieży
            tutorialShown = true;
        }

        return false;
    }

    private void ClearSelection()
    {
        rangeVisualizer.Clear();
        towerInfoPanel.SetActive(false);
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