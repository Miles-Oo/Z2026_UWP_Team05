using UnityEngine;
using System.Linq;

public class RoadDex : MonoBehaviour
{
    [SerializeField] private int stepIndex = 0;
    public int StepIndex => stepIndex;

    public Transform GetWayPointPos() => transform;
}