using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PW_InputManager : MonoBehaviour {

    private BoxCollider2D boxCollider;
    private ContactFilter2D contactFilter;
    private PW_ScoreManager scoreManager;
    private PW_SheepMovement[] sheeps;

    public float boostTime = 10f;
    public float boostFull = 10f;
    public float precisionBonus = 1;
    public float boostForce = 5f;
    public PW_SheepMovement player;
    public PW_SheepMovement enemy;

    public float turnTimeInSec = 60f;

    private float currentPrecBonus = 0;
    private float startTime = 0f;
    private float boost = 0f;
    private bool lastPrecCheck = false;
    private bool boostIsRunning = false;

    private void Awake()
    {
        scoreManager = FindObjectOfType<PW_ScoreManager>();
        boxCollider = GetComponent<BoxCollider2D>();
        contactFilter = new ContactFilter2D
        {
            layerMask = 9
        };

        sheeps = new PW_SheepMovement[] { player, enemy };
    }

    private void Start()
    {
        foreach (PW_SheepMovement sheep in sheeps)
        {
            sheep.StartGame();
        }
    }

    private void Update()
    {
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

        ///control the time for one round of play
        startTime += Time.deltaTime;
        if (startTime >= turnTimeInSec)
        {
            print("Done");
            foreach (PW_SheepMovement sheep in sheeps)
            {
                sheep.StopGame();
            }
            startTime = 0f;
            //evaluate Score and give corresponding win/lose messages
        }
    }

    private void CheckForDirection(Direction inputDir)
    {
        Collider2D[] touchingColliders = new Collider2D[20];
        int colliderAmount = boxCollider.OverlapCollider(contactFilter.NoFilter(), touchingColliders);

        if (colliderAmount == 1 && touchingColliders[0] != null)
        {
            print("Currently registered collider: " + touchingColliders[0].gameObject);
            if(inputDir == touchingColliders[0].GetComponent<PW_Direction>().direction)
            {
                print("correct");
                PrecisionCheck(touchingColliders[0].gameObject);
                lastPrecCheck = true;
            }
            else
            {
                print("wrong button pressed");
                scoreManager.SubstractScore(ScoreMalus.WrongDirPressed);
                lastPrecCheck = false;
                enemy.EnemyPushHelper();
            }
            //do some effect instead of just disabling
            touchingColliders[0].gameObject.SetActive(false);
            print("disabled");
            
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
        print(deltaY);
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

            print("did not hit a direction in time");
        }
    }
}