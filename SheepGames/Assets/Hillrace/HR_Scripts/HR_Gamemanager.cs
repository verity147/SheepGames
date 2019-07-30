using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class HR_Gamemanager : MonoBehaviour
{
    //remember to reset this on restart
    private int pointsCollected = 0;

    internal void CollectPoint()
    {
        pointsCollected += 1;
        //add UI counter
    }
}
