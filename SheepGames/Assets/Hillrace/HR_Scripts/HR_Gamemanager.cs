using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using TMPro;
using Cinemachine;
using UnityEngine.SceneManagement;

public class HR_Gamemanager : MonoBehaviour
{

    public HR_Player player;
    public CinemachineVirtualCamera virtualCamera;
    public HR_BonbonManager bonbonManager;
    public TMP_Text timeScore;
    public TMP_Text bonbonScore;
    public Button readyButton;
    public Button continueButton;
    public Image pauseMenu;
    public Image pauseMenu_T;
    public Animator goalSheep;
    public Vector3 goalPosition;
    public SpectatorHandler spectatorHandler;
    public float goalAnimTime = 1f;

    private int pointsCollected = 0;
    private float gameTimer = 0;
    private bool gameIsRunning = false;
    private Vector3 playerStartPos;
    private bool gameOver = false;
    private int newScore;
    private float goalMoveTimer = 0f;
    private Vector3 playerGoalStartPos;
    private bool cursorMoving = false;
    private float cursorFadeTimer = 0f;
    private readonly float maxTimeScore = 1350f; ///score should be slightly over 1k for perfect play

    private void Start()
    {
        playerStartPos = player.gameObject.transform.position;
    }
   
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && gameIsRunning)
        {
            StartPauseGame(false);
            if (TournamentTracker.IsTournamentRunning())
            {
                pauseMenu_T.gameObject.SetActive(true);
            }
            else
            {
                pauseMenu.gameObject.SetActive(true);
            }
        }
        if (gameTimer > 3550f)
        {
            StopGame(); ///makes sure the timer can't go over one hour
            ResetGame();
        }

        cursorMoving = Input.GetAxis("Mouse X") < 0 || (Input.GetAxis("Mouse X") > 0);

        if (gameIsRunning)
        {
            gameTimer += Time.deltaTime;
            timeScore.text = TimeSpan.FromSeconds(gameTimer).ToString(@"mm\:ss\:ff"); ///formatting the countdown output
            ManageCursorVisibility();
        }
        if (gameOver)
        {
            MovePlayerInGoal();
            spectatorHandler.EndOfGameReaction(WinState.Win);
        }
    }

    private void ManageCursorVisibility()
    {
        if (cursorMoving && !Cursor.visible)
        {
            Cursor.visible = true;
        }
        if (!cursorMoving && Cursor.visible)
        {
            cursorFadeTimer += Time.deltaTime;
            if (cursorFadeTimer >= 1f)
            {
                Cursor.visible = false;
                cursorFadeTimer = 0f;
            }
        }
    }

    internal void CollectPoint()
    {
        pointsCollected += 1;
        player.TriggerSound(HR_Player.HR_SoundList.score);
        bonbonScore.text = string.Format("{0} / {1}", pointsCollected, bonbonManager.childCount);
    }

    public void StartPauseGame(bool go)
    {
        gameIsRunning = go;
        player.GameRunning = gameIsRunning;
        Cursor.visible = !go;
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
        player.myAnimator.SetTrigger("goal");
        goalSheep.SetTrigger("wave");
        playerGoalStartPos = player.transform.position;
        newScore = Mathf.RoundToInt(maxTimeScore - gameTimer) + pointsCollected;
        DataCollector.UpdateScore(newScore);
    }

    public void ResetGame()
    {       
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == player.gameObject && !gameOver)
        {
            gameOver = true;
            StartPauseGame(false);
            StopGame();
            continueButton.gameObject.SetActive(true);
        }
    }
}
