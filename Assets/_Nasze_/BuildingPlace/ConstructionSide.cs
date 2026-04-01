using UnityEngine;

public class ConstructionSide : MonoBehaviour
{
    private GameObject placedTower;

    public GameObject GetPlacedTower() => placedTower;

    private bool placeTaken = false;
    public bool IsFree() => !placeTaken;

    public void SetTower(GameObject tower)
    {
        placedTower = tower;
        placeTaken = tower != null;
    }
}