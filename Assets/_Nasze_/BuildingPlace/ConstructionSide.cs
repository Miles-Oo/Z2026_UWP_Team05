using UnityEngine;

public class ConstructionSide : MonoBehaviour
{
    private GameObject placedTower;
    public GameObject GetPlacedTower(){return placedTower;}
    private bool placeTaken = false;

    public bool IsFree()
    {
        return !placeTaken;
    }
    public void SetTower(GameObject gameObject){

        if (gameObject == null)
        {
            Destroy(placedTower);
            placedTower=null;
            placeTaken=false;
        }
        else
        {
            placedTower=gameObject;
            placeTaken=true;
        }
      
        }
}