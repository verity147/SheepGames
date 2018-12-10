using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PW_InputManager : MonoBehaviour {

    private BoxCollider2D boxCollider;
    private ContactFilter2D contactFilter;
    private PW_ScoreManager scoreManager;
    private PW_DirectionsSpawner directionsSpawner;
    private PW_Timer timer;

    public float boostTime = 10f;
    public float boostFull = 10f;
    public float precisionBonus = 1;
    public float boostForce = 5f;
    public PW_SheepMovement player;
    public PW_SheepMovement enemy;
    public Slider pBar;
    public GameObject continueButton;
    public SpectatorHandler spectatorHandler;

    private float currentTurnTime = 0;
    private float currentPrecBonus = 0;
    private float boost = 0f;
    private bool lastPrecCheck = false;
    private bool boostIsRunning = false;
    private float playerMoveDist;
    private float playerCurrentDist;

    internal bool gameIsRunning = false;

    private void Awake()
    {
        scoreManager = FindObjectOfType<PW_ScoreManager>();
        directionsSpawner = FindObjectOfType<PW_DirectionsSpawner>();
        timer = FindObjectOfType<PW_Timer>();
        boxCollider = GetComponent<BoxCollider2D>();
        contactFilter = new ContactFilter2D
        {
            layerMask = 9
        };
    }

    private void Start()
    {
        playerMoveDist = Mathf.Abs(player.winPosX - player.losePosX);
    }

    private void Update()
    {
        if (!gameIsRunning)
                return;
        currentTurnTime += Time.deltaTime;
        pBar.value = Mathf.Abs(player.transform.position.x - player.losePosX) / playerMoveDist;
        if (Input.GetButtonDown("Left"))
        {
            CheckForDirection(Direction.Left);
        }
        if (Input.GetButtonDown("Right"))
        {
            CheckForDirection(Direction.Right);
        }
        if (Input.GetButtonDown("Up"))
        {
            CheckForDirection(Direction.Up);
        }
        if (Input.GetButtonDown("Down"))
        {
            CheckForDirection(Direction.Down);
        }
    }

    internal void StartGame()
    {
        gameIsRunning = true;
        player.StartGame();
        enemy.StartGame();
        directionsSpawner.StartSpawnEngine();
        timer.SetUpTimer();
    }

    internal void EndGame(WinState winState)
    {
        gameIsRunning = false;
        //evaluate Score, and trigger corresponding win/lose messages

        int timeRemaining = Mathf.RoundToInt(timer.turnTimeInSec - currentTurnTime);

        switch (winState)
        {
            case WinState.Loss:
                player.StopGame(WinState.Loss);
                enemy.StopGame(WinState.Win);
                break;
            case WinState.Win:
                player.StopGame(WinState.Win);
                enemy.StopGame(WinState.Loss);
                scoreManager.UpdateScore(timeRemaining * 10);
                break;
            case WinState.Neutral:
                player.StopGame(WinState.Neutral);
                enemy.StopGame(WinState.Neutral);
                break;
            default:
                break;
        }

        directionsSpawner.StopAllCoroutines();
        timer.StopAllCoroutines();
        timer.gameStarted = false;
        continueButton.SetActive(true);
        spectatorHandler.EndOfGameReaction(winState);
    }

    private void CheckForDirection(Direction inputDir)
    {
        Collider2D[] touchingColliders = new Collider2D[20];
        int colliderAmount = boxCollider.OverlapCollider(contactFilter.NoFilter(), touchingColliders);

        if (colliderAmount == 1 && touchingColliders[0] != null)
        {
            //print("Currently registered collider: " + touchingColliders[0].gameObject);
            if(inputDir == touchingColliders[0].GetComponent<PW_Direction>().direction)
            {
                PrecisionCheck(touchingColliders[0].gameObject);
                lastPrecCheck = true;
            }
            else
            {
                scoreManager.SubstractScore(ScoreMalus.WrongDirPressed);
                lastPrecCheck = false;
                enemy.EnemyPushHelper();
            }
            ///correct input causes the direction to be disabled with its effect played
            touchingColliders[0].GetComponent<PW_Direction>().Disable();
        }
        else if(colliderAmount>1)
        {
            ///at least 0.45 seems a sensible time for min. Direction spawn wait time
            Debug.LogError("DirectionsSpawner's min. spawn wait time is too low, more than one direction touchend the Input Manager!");
        }
        else
        {
            ///did not hit any direction
            scoreManager.SubstractScore(ScoreMalus.PressedNoDir);
            print("pressed a button but there was no direction");
            //do an effect
        }
    }

    private void PrecisionCheck(GameObject direction)
    {
        float deltaY = Mathf.Abs(direction.transform.position.y - transform.position.y);
        ///the multiplier should never diminish the score, so it gets clamped with a min of 1
        float scoreMult = 1f;
        if (deltaY > 0.05f)
        {
            scoreMult = Mathf.Clamp((scoreManager.maxScoreMult - deltaY), 1f, scoreManager.maxScoreMult);
        }
        else
        {
            ///if the player was particularly precise, he gets the maximum multiplier possible
            scoreMult = scoreManager.maxScoreMult;
        }
        scoreManager.AddScore(scoreMult);
        PrecisionBoost(scoreMult);
        if (boostIsRunning)
        {
            player.Push(scoreMult, boostForce);
        }
        else
        {
            player.Push(scoreMult);
        }
    }

    private void PrecisionBoost(float scoreMult)
    {
        if (boostIsRunning)
        {
            return;
        }
        if (lastPrecCheck)
        {
            currentPrecBonus += precisionBonus;
        }
        else if (!lastPrecCheck)
        {
            currentPrecBonus = 0f;
        }

        if (boost < boostFull)
        {
            boost += currentPrecBonus + scoreMult;
        }
        else if (boost >= boostFull)
        {
            boostIsRunning = true;
            StartCoroutine(Boost());
            //start visual representation of boost
        }
    }

    IEnumerator Boost()
    {
        yield return new WaitForSeconds(boostTime);
        boost = 0f;
        boostIsRunning = false;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        GameObject collidedObject = collision.gameObject;
        //do effect to show the direction was missed
        if (collidedObject.activeInHierarchy)
        {
            collidedObject.SetActive(false);
            scoreManager.SubstractScore(ScoreMalus.DirMissed);
            enemy.EnemyPushHelper();
        }
    }
}