using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class TowerManagerNew : MonoBehaviour
{
    [SerializeField] private GameObject towerPrefab;
    public GameObject GetTower(){return towerPrefab;}
     private GameObject towerPreviewPrefab;
     [SerializeField] private GameObject hammerAsset;
    [SerializeField] private Money money;
    private int towerCost;

    private bool isBuildMode = false;
    private bool isSellMode = false;
    private GameObject towerPreview;

    void Start()
    {
        towerCost=towerPrefab.GetComponent<TowerPrice>().GetPrice();
        towerPreviewPrefab=towerPrefab;
        towerPreviewPrefab.GetComponent<TowerAttack>().enabled=false;
    }
    void Update()
    {
        if (Keyboard.current.bKey.wasPressedThisFrame)
        {
            EnterBuildMode();
        }
 if (Keyboard.current.vKey.wasPressedThisFrame)
        {
          SellTower();
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
    public void EnterSellingMode()
    {
        isSellMode = true;
        towerPreview = Instantiate(hammerAsset);
        Debug.Log("Tryb sprzedawania ON");
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

    Vector3 pos;

    Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
    if (groundPlane.Raycast(ray, out float distance))
    {
        pos = ray.GetPoint(distance);
        pos.y = 0f;
    }
    else
    {
        return;
    }

   
    if (Physics.Raycast(ray, out RaycastHit hit,100,layerMaskg))
    {
        ConstructionSide site = hit.collider.GetComponent<ConstructionSide>();

        if (site != null && site.IsFree())
        {
          
            pos = hit.collider.transform.position;
        }
    }

    towerPreview.transform.position = pos;
}
[SerializeField] LayerMask layerMaskg;
void PlaceTower()
{
    Vector2 mousePos = Mouse.current.position.ReadValue();
    Ray ray = Camera.main.ScreenPointToRay(mousePos);

    if (Physics.Raycast(ray, out RaycastHit hit,100,layerMaskg))
    {
        ConstructionSide site = hit.collider.GetComponent<ConstructionSide>();

        if (site != null && site.IsFree())
        {
            if (money.GetCurrMoney() >= towerCost)
            {
                money.SubMoney(towerCost);

               GameObject g= Instantiate(towerPrefab, hit.collider.transform.position, Quaternion.identity);
                site.SetTower(g);
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
public void SellTower(){
            Vector2 mousePos = Mouse.current.position.ReadValue();
    Ray ray = Camera.main.ScreenPointToRay(mousePos);

    if (Physics.Raycast(ray, out RaycastHit hit,100,layerMaskg))
    {
        ConstructionSide site = hit.collider.GetComponent<ConstructionSide>();

        if (site != null && !site.IsFree()){
            
              money.AddMoney(towerCost/2);
              site.SetTower(null);
        }
    }
       
    }
}