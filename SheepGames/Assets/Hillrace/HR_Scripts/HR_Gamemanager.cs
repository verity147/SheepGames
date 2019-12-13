using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class HR_Gamemanager : MonoBehaviour
{
    //remember to reset this on restart
    private int pointsCollected = 0;

    public HR_Player player;

    internal void CollectPoint()
    {
        pointsCollected += 1;
        //add UI counter
    }

    public void StartGame()
    {
        player.gameRunning = true;

    }

    private void Update()
    {
        
    }
}
//press start/ready button
//countdown
//enable player control
//start timer
//menu on escape
//touch finishline
//disable player control
//stop timer
//show button to display highscore