using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class TowerHover : MonoBehaviour
{
    [SerializeField] private SlowTowerHoverInfo hoverUI;

    private SlowTowerController currentSlowTower;
    private GameObject currentBasicTower;

    void Update()
    {
        if (hoverUI == null) return;

        if (EventSystem.current != null &&
            EventSystem.current.IsPointerOverGameObject())
            return;

        Vector2 mousePos = Mouse.current.position.ReadValue();
        Ray ray = Camera.main.ScreenPointToRay(mousePos);

        if (Physics.Raycast(ray, out RaycastHit hit, 100f))
        {
            // 🔧 FIX 1: zawsze bierz ROOT wieży przez TowerPrice (stabilny identyfikator)
            var towerRoot = hit.collider.GetComponentInParent<TowerPrice>()?.gameObject;

            if (towerRoot == null)
            {
                Hide();
                return;
            }

            // 🟣 SLOW TOWER
            var slowTower = towerRoot.GetComponent<SlowTowerController>();
            if (slowTower != null)
            {
                currentBasicTower = null;

                if (currentSlowTower != slowTower)
                {
                    currentSlowTower = slowTower;

                    Debug.Log($"[HOVER] SlowTower level = {slowTower.GetCurrentLevel()}");

                    hoverUI.Show(
                        slowTower.CurrentStats,
                        slowTower.GetCurrentLevel()
                    );
                }

                return;
            }

            // 🟢 BASIC TOWER
            var attack = towerRoot.GetComponent<TowerAttack>();
            var price = towerRoot.GetComponent<TowerPrice>();

            if (attack != null && price != null)
            {
                currentSlowTower = null;

                // 🔧 FIX 2: stabilne porównanie rootów
                if (currentBasicTower != towerRoot)
                {
                    currentBasicTower = towerRoot;

                    Debug.Log("[HOVER] Basic tower detected");

                    hoverUI.ShowBasicTower(
                        attack.GetDamage(),
                        attack.GetRange(),
                        attack.GetAttackInterval(),
                        price.GetLevel(),
                        price.GetUpgradeCost()
                    );
                }

                return;
            }
        }

        Hide();
    }

    private void Hide()
    {
        currentSlowTower = null;
        currentBasicTower = null;
        hoverUI.Hide();
    }
}