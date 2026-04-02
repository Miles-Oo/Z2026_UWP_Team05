using UnityEngine;

public class ConstructionSide : MonoBehaviour
{
   [SerializeField] private GameObject placedTower;

    public GameObject GetPlacedTower() => placedTower;

    private bool placeTaken = false;
    public bool IsFree() => !placeTaken;

    public void SetTower(GameObject tower)
    {
        if (tower == null) { 
            Destroy(placedTower); 
            placedTower=null; 
            placeTaken=false; 
            }
        else {
            placedTower=tower;
            placeTaken=true; 
            }
    }
}