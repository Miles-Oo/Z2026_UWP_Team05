using UnityEngine;
using UnityEngine.InputSystem; // nowy Input System

public class TowerManager : MonoBehaviour
{
    [SerializeField] private GameObject towerPrefab;
    [SerializeField] private Money money;
    [SerializeField] private int towerCost = 10;

    private GameObject towerPreview;

    void Update()
    {
        // Sprawdzamy lewy klik myszy w nowym Input System
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            PlaceTower();
        }
    }

    void PlaceTower()
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();
        Ray ray = Camera.main.ScreenPointToRay(mousePos);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Vector3 pos = hit.point;

            // Ustawiamy dol wieży na Z=0
            // Zakładamy, że pivot wieży jest w środku → dodajemy połowę wysokości collidera
            Collider towerCollider = towerPrefab.GetComponent<Collider>();
            float heightOffset = 0f;
            if (towerCollider != null)
            {
                heightOffset = towerCollider.bounds.extents.y;
            }

            pos.y = heightOffset; // teraz dol wieży jest na y=0

            if (money.GetCurrMoney() >= towerCost)
            {
                money.SubMoney(towerCost);
                Instantiate(towerPrefab, pos, Quaternion.identity);
                Debug.Log("Wieża postawiona na ziemi!");
            }
            else
            {
                Debug.Log("Nie masz wystarczająco pieniędzy!");
            }
        }
    }
}