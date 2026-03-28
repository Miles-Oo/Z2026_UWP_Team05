
using UnityEngine;
using UnityEngine.InputSystem;

public class TowerManagerNewPlus : MonoBehaviour
{
    [SerializeField] private GameObject towerPrefab;
    public GameObject GetTower(){return towerPrefab;}
     private GameObject towerPreviewPrefab;
     [SerializeField] private GameObject hammerAsset;
    [SerializeField] private Money money;

    [SerializeField] LayerMask layerMaskg;
    private int towerCost;
    private GameObject objPreview;


    private Mode currentMode;

    void Start(){
        towerCost=towerPrefab.GetComponent<TowerPrice>().GetPrice();
        towerPreviewPrefab=towerPrefab;
        towerPreviewPrefab.GetComponent<TowerAttack>().enabled=false;
        EnterMode(Mode.IDLE);
    }
    public void EnterMode(Mode mode){currentMode=mode;}
    public void ExitMode(){

        switch (currentMode)
        {
            case Mode.BUILD:
            ExitPrewMode();
            EnterMode(Mode.IDLE);
            break;
            case Mode.SELL:
            ExitPrewMode();
            EnterMode(Mode.IDLE);
            break;
            case Mode.UPGRADE:
            ExitPrewMode();
            EnterMode(Mode.IDLE);
            break;
            default:
            EnterMode(Mode.IDLE);
            break;
        }
        currentMode=Mode.IDLE;
        }

    void Update(){
        if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            ExitMode();
        }

        if (currentMode!=Mode.IDLE)
        {
            MovePreview();
        }
        if (currentMode==Mode.BUILD && Mouse.current.leftButton.wasPressedThisFrame)
        {
            PlaceTower();
        }
        if (currentMode==Mode.SELL && Mouse.current.leftButton.wasPressedThisFrame)
        {
            SellTower();
        }
        if (currentMode==Mode.UPGRADE && Mouse.current.leftButton.wasPressedThisFrame)
        {
            UpgradeTower();
        }
    }
    /// <summary>
    /// DLA PRZYCISKÓW NA UI
    /// </summary>
    /// 
    /// 
    /// 
    /// 
    /// 
    /// 
    /// 
    /// 
    /// 
    /// 
    /// NIECH UŻYWA ASETU Z OBAZKA ABY WYGLĄDAŁO ŻE TEGO UŻYWA A PO UŻYCIU SETUJE NA SWOJE MIEJSCE
    /// 
    /// 
    /// 
    /// 
    /// 
    /// 
    /// 
    /// 
    public void EnterBuildMode()
    {
        EnterMode(Mode.BUILD);
        objPreview = Instantiate(towerPreviewPrefab);
        Debug.Log("Tryb budowania ON");
    }
    public void EnterSellMode()
    {
        EnterMode(Mode.SELL);
        objPreview = Instantiate(hammerAsset);
        Debug.Log("Tryb sprzedawania ON");
    }
    public void EnterUpgradeMode()
    {
        EnterMode(Mode.UPGRADE);
        objPreview = Instantiate(hammerAsset);
        Debug.Log("Tryb ulepszania ON");
    }
    void ExitPrewMode(){
        if (objPreview != null)
        {
            Destroy(objPreview);
            objPreview = null;
        }
        Debug.Log("Tryb prev OFF");
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

    objPreview.transform.position = pos;
}

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
                ExitMode();
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
              ExitMode();
        }
    }
       
    }

    public void UpgradeTower()
    {
        Debug.Log("ulep");
    }
}