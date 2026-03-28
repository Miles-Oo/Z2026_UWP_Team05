using UnityEngine;
using UnityEngine.InputSystem;

public class TowerManager : MonoBehaviour
{
    [SerializeField] private GameObject towerPrefab;
    [SerializeField] private GameObject towerPreviewPrefab;
    [SerializeField] private Money money;
    [SerializeField] private int towerCost = 10;

    private bool isBuildMode = false;
    private GameObject towerPreview;

    void Update()
    {
        if (Keyboard.current.bKey.wasPressedThisFrame)
        {
            EnterBuildMode();
        }

        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            ExitBuildMode();
        }

        if (isBuildMode && towerPreview != null)
        {
            MovePreview();
        }

        if (isBuildMode && Mouse.current.leftButton.wasPressedThisFrame)
        {
            PlaceTower();
        }
    }

    public void EnterBuildMode()
    {
        isBuildMode = true;

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