using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using TMPro;
using Cinemachine;

public class HR_Gamemanager : MonoBehaviour
{
    //remember to reset this on restart

    public HR_Player player;
    public CinemachineVirtualCamera virtualCamera;
    public HR_BonbonManager bonbonManager;
    public TMP_Text timeScore;
    public TMP_Text bonbonScore;
    public Button readyButton;
    public Button continueButton;
    public Image pauseMenu;
    public Vector3 goalPosition;
    public float goalAnimTime = 1f;

    private int pointsCollected = 0;
    private float gameTimer = 0;
    private bool gameIsRunning = false;
    private Vector3 playerStartPos;
    private bool gameOver = false;
    private float goalMoveTimer = 0f;
    private Vector3 playerGoalStartPos;


    private void Start()
    {
        playerStartPos = player.gameObject.transform.position;
    }
   
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pauseMenu.gameObject.SetActive(true);
            StartPauseGame(false);
        }
        if (gameTimer > 3550f)
        {
            StopGame(); ///makes sure the timer can't go over one hour
            ResetGame();
        }
        if (gameIsRunning)
        {
            gameTimer += Time.deltaTime;
            timeScore.text = TimeSpan.FromSeconds(gameTimer).ToString(@"mm\:ss\:ff"); ///formatting the countdown output
        }

        if (gameOver)
            MovePlayerInGoal();
    }

    internal void CollectPoint()
    {
        pointsCollected += 1;
        bonbonScore.text = string.Format("{0} / {1}", pointsCollected, bonbonManager.childCount);
    }

    public void StartPauseGame(bool go)
    {
        gameIsRunning = go;
        player.GameRunning = gameIsRunning;
    }

    private void MovePlayerInGoal()
    {
         goalMoveTimer += Time.deltaTime;
         player.transform.position = Vector3.Lerp(playerGoalStartPos, goalPosition, goalMoveTimer);
        
        if (goalMoveTimer >= goalAnimTime)
        {
            gameOver = false;
        }
    }

    private void StopGame()
    {
        StartPauseGame(false);
        player.myAnimator.SetTrigger("goal");
        playerGoalStartPos = player.transform.position;
        gameOver = true;
    }

    public void ResetGame()
    {
        bonbonManager.ResetBonbons();
        gameTimer = 0f;
        pointsCollected = 0;
        timeScore.text = "00:00:00";
        bonbonScore.text = "0";
        Vector3 oldPlayerPos = player.transform.position;
        Vector3 playerMoveDelta = oldPlayerPos - playerStartPos;
        player.transform.position = playerStartPos;
        if(player.transform.lossyScale.x < 0)
        {
            player.Flip();
        }
        virtualCamera.OnTargetObjectWarped(player.transform, playerMoveDelta);
        readyButton.gameObject.SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == player.gameObject)
        {
            StopGame();
            continueButton.gameObject.SetActive(true);
        }
    }
}
//press start/ready button +
//countdown
//enable player control +
//start timer +
//touch finishline +
//disable player control + 
//stop timer + 
//show button to display highscore

//make escape show pause menu