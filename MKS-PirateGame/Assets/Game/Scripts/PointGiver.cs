using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointGiver : MonoBehaviour
{
    [SerializeField] private int points_to_give = 10;

    public int GetPointsToGive(){
        return points_to_give;
    }
}
