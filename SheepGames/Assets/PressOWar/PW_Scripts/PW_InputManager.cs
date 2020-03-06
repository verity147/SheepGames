using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

enum FeedbackSounds { correct, wrong }

public class PW_InputManager : MonoBehaviour {

    private BoxCollider2D boxCollider;
    private ContactFilter2D contactFilter;
    private PW_ScoreManager scoreManager;
    private PW_DirectionsSpawner directionsSpawner;
    private PW_Timer timer;
    private AudioSource audioSource;

    public float boostTime = 10f;
    public float boostFull = 10f;
    public float precisionBonus = 1;
    public float boostForce = 5f;
    public int lossPenalty = -100;
    public PW_SheepMovement player;
    public PW_SheepMovement enemy;
    public Slider pBar;
    public GameObject continueButton;
    public SpectatorHandler spectatorHandler;
    public AudioClip correctSound;
    public AudioClip wrongSound;

    private float currentTurnTime = 0;
    private float currentPrecBonus = 0;
    private float boost = 0f;
    private bool lastPrecCheck = false;
    private bool boostIsRunning = false;
    private float playerMoveDist;

    internal bool gameIsRunning = false;
    private bool cursorMoving = false;
    private float cursorFadeTimer = 0f;

    private readonly string winTextKey = "Win_T";
    private readonly string loseTextKey = "Loss_T";
    private readonly string outOfTimeTextKey = "TimeUp_T";

    private void Awake()
    {
        scoreManager = FindObjectOfType<PW_ScoreManager>();
        directionsSpawner = FindObjectOfType<PW_DirectionsSpawner>();
        timer = FindObjectOfType<PW_Timer>();
        boxCollider = GetComponent<BoxCollider2D>();
        audioSource = GetComponent<AudioSource>();
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

        cursorMoving = Input.GetAxis("Mouse X") < 0 || (Input.GetAxis("Mouse X") > 0);
        ManageCursorVisibility();
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

    internal void StartGame()
    {
        gameIsRunning = true;
        player.StartGame();
        enemy.StartGame();
        directionsSpawner.StartSpawnEngine();
        timer.SetUpTimer();
        Cursor.visible = false;
    }

    internal void EndGame(WinState winState)
    {
        gameIsRunning = false;
        Cursor.visible = true;
        //evaluate Score, and trigger corresponding win/lose messages

        int timeRemaining = Mathf.RoundToInt(timer.turnTimeInSec - currentTurnTime);

        switch (winState)
        {
            case WinState.Loss:
                player.StopGame(WinState.Loss);
                enemy.StopGame(WinState.Win);
                scoreManager.UpdateScore(lossPenalty);
                continueButton.GetComponentInChildren<LocalizedText>().key = loseTextKey;
                break;
            case WinState.Win:
                player.StopGame(WinState.Win);
                enemy.StopGame(WinState.Loss);
                scoreManager.UpdateScore(timeRemaining * 10);
                continueButton.GetComponentInChildren<LocalizedText>().key = winTextKey;
                break;
            case WinState.Neutral:
                player.StopGame(WinState.Neutral);
                enemy.StopGame(WinState.Neutral);
                scoreManager.UpdateScore(0);    ///running out of time incurs neither bonus nor penalty
                continueButton.GetComponentInChildren<LocalizedText>().key = outOfTimeTextKey;
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
            if(inputDir == touchingColliders[0].GetComponent<PW_Direction>().direction)///correct button pressed
            {
                audioSource.PlayOneShot(correctSound);
                PrecisionCheck(touchingColliders[0].gameObject);
                lastPrecCheck = true;
            }
            else///wrong button pressed
            {
                scoreManager.SubstractScore(ScoreMalus.WrongDirPressed);
                audioSource.PlayOneShot(wrongSound);
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
            audioSource.PlayOneShot(wrongSound);
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
        if (collidedObject.activeInHierarchy)
        {
            collidedObject.SetActive(false);
            audioSource.PlayOneShot(wrongSound);
            scoreManager.SubstractScore(ScoreMalus.DirMissed);
            enemy.EnemyPushHelper();
        }
    }
}