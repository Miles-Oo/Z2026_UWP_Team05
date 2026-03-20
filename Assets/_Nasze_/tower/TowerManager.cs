using UnityEngine;
using UnityEngine.InputSystem;

public class TowerManager : MonoBehaviour
{
    [SerializeField] private GameObject towerPrefab;          // prawdziwa wieża
    [SerializeField] private GameObject towerPreviewPrefab;   // ghost
    [SerializeField] private Money money;
    [SerializeField] private int towerCost = 10;

    private bool isBuildMode = false;
    private GameObject towerPreview;

    void Update()
    {
        // Włączenie trybu budowania
        if (Keyboard.current.bKey.wasPressedThisFrame)
        {
            EnterBuildMode();
        }

        // Anulowanie
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            ExitBuildMode();
        }

        // Ruch preview
        if (isBuildMode && towerPreview != null)
        {
            MovePreview();
        }

        // Klik = postawienie wieży
        if (isBuildMode && Mouse.current.leftButton.wasPressedThisFrame)
        {
            PlaceTower();
        }
    }

    void EnterBuildMode()
    {
        isBuildMode = true;

        // Tworzymy preview (osobny prefab!)
        towerPreview = Instantiate(towerPreviewPrefab);

        Debug.Log("Tryb budowania ON");
    }

    void ExitBuildMode()
    {
        isBuildMode = false;

        if (towerPreview != null)
        {
            Destroy(towerPreview);
            towerPreview = null;
        }

        Debug.Log("Tryb budowania OFF");
    }

    void MovePreview()
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();
        Ray ray = Camera.main.ScreenPointToRay(mousePos);

        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);

        if (groundPlane.Raycast(ray, out float distance))
        {
            Vector3 pos = ray.GetPoint(distance);

            Collider towerCollider = towerPrefab.GetComponent<Collider>();
            float heightOffset = towerCollider != null ? towerCollider.bounds.extents.y : 0f;

            pos.y = heightOffset;

            towerPreview.transform.position = pos;
        }
    }

    void PlaceTower()
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();
        Ray ray = Camera.main.ScreenPointToRay(mousePos);

        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);

        if (groundPlane.Raycast(ray, out float distance))
        {
            Vector3 pos = ray.GetPoint(distance);

            Collider towerCollider = towerPrefab.GetComponent<Collider>();
            float heightOffset = towerCollider != null ? towerCollider.bounds.extents.y : 0f;

            pos.y = heightOffset;

            if (money.GetCurrMoney() >= towerCost)
            {
                money.SubMoney(towerCost);

                Instantiate(towerPrefab, pos, Quaternion.identity);

                Debug.Log("Wieża postawiona!");

                ExitBuildMode();
            }
            else
            {
                Debug.Log("Nie masz wystarczająco pieniędzy!");
            }
        }
    }
}