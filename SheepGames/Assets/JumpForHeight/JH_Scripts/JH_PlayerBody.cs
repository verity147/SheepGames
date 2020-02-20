using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum JH_SoundList { Step, Step2, Flight }


public class JH_PlayerBody : MonoBehaviour
{
    public float maxRunwayDist;
    ///has to be same length as RunUp animation clip
    public float runUpDuration;
    public float jumpPowerMod = 9f;
    public float jumpPowerModX = 0.9f;
    public float jumpPowerModY = 1.3f;
    public float resetDist = 0.5f;
    public float moveSpeed = 10f;
    public ParticleSystem dustParticle;
    public Transform[] feetPositions;
    public AudioClip[] audioClips;

    private SpectatorHandler spectatorHandler;
    private JH_GameController gameController;
    private JH_UIManager uIManager;
    internal Animator anim;
    private Rigidbody2D playerRB;
    private AudioSource audioSource;

    private float speed;
    private float maxRunLeft;
    private float movedDist;

    internal bool timeForRunUp = false;
    private bool hasJumped = false;

    internal Vector2 flightVel;
    private Vector3 lastPos = Vector3.zero;
    private Vector2 startPos;
    private Vector2 currentMousePos = Vector2.zero;
    internal Vector2 jumpForce = Vector2.zero;
    private Vector2 jumpPos;
    internal bool interactable = true;

    private void Awake()
    {
        spectatorHandler = FindObjectOfType<SpectatorHandler>();
        gameController = FindObjectOfType<JH_GameController>();
        uIManager = FindObjectOfType<JH_UIManager>();
        anim = GetComponent<Animator>();
        playerRB = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        startPos = gameController.transform.position;
    }
    private void Start()
    {
        maxRunLeft = startPos.x - maxRunwayDist;
        ///trying to account for the offset the player has from the Start position
        jumpPos = new Vector2(startPos.x - 0.4f, transform.position.y - 0.2f);
    }
    private void Update()
    {
        currentMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    private void OnMouseDown()
    {
        if (!hasJumped)
        {
            uIManager.ToggleExplanation(false);
        }
    }

    private void OnMouseDrag()
    {
        if (!interactable)
            return;
        if (!hasJumped)
        {
            movedDist = Mathf.Abs(transform.position.x - gameController.transform.position.x);

            if (currentMousePos.x <= startPos.x && currentMousePos.x > maxRunLeft)
            {
                transform.position = Vector3.MoveTowards(transform.position, 
                                                        new Vector2(currentMousePos.x,transform.position.y), 
                                                        moveSpeed * Time.deltaTime);
            }else if(currentMousePos.x < maxRunLeft)
            {
                transform.position = Vector3.MoveTowards(transform.position, 
                                                        new Vector2(maxRunLeft, transform.position.y), 
                                                        moveSpeed * Time.deltaTime);
            }else if (currentMousePos.x > startPos.x)
            {
                transform.position = Vector3.MoveTowards(transform.position, 
                                                        new Vector2(startPos.x, transform.position.y), 
                                                        moveSpeed * Time.deltaTime);
            }
            speed = (lastPos - transform.position).magnitude;
            anim.SetFloat("moveSpeed", speed);
            lastPos = transform.position;
            CalculateJumpForce();
            gameController.DrawTrajectoryPoints(jumpPos, jumpForce / playerRB.mass*playerRB.gravityScale);

        }
    }

    public void PlayDustParticles(int position)
    {
        Instantiate(dustParticle, feetPositions[position]);
    }

    private void OnMouseUp()
    {
        if (!interactable)
            return;
        gameController.drawNow = false;
        if (movedDist > resetDist)
        {
            hasJumped = true;
            MovePlayerToStart();      
            uIManager.CountTries();
            interactable = false;
        }
        else
        {
            ///if the player has barely moved from spawn it is assumed it was unintentional & give explanation
            gameController.SpawnNewPlayer();
            uIManager.ToggleExplanation(true);
        }
    }

    private void CalculateJumpForce()
    {
        ///take mouse angle relative to the x-axis
        float mouseAngle = Mathf.Atan2(currentMousePos.y, currentMousePos.x);
        ///translate angle into vector
        Vector2 jumpDirection = new Vector2(Mathf.Abs((Mathf.Cos(mouseAngle))), Mathf.Abs((Mathf.Sin(mouseAngle))));
        ///multiply with player distance from start and power modificator
        float jumpX = jumpDirection.x * (movedDist * 10f) * jumpPowerModX;
        float jumpY = jumpDirection.y * (movedDist * 10f) * jumpPowerModY;
        jumpForce = new Vector2(jumpX, jumpY) * jumpPowerMod;
    }

    internal void MovePlayerToStart()
    {
        if (transform.position.x < startPos.x)
        {
            anim.SetTrigger("playerRelease");
            audioSource.Stop();
            //PlayAudio(1);
            ///x position can put playerBody backwards if runwayDist is too short
            if (gameController.transform.position.x >= transform.position.x)
            {
                float distance = jumpPos.x - transform.position.x;
                float speed = distance / runUpDuration;
                playerRB.velocity = new Vector2(speed, 0f);
            }
            else
            {
                ///if player is somehow to the right of start, respawn
                gameController.SpawnNewPlayer();
                uIManager.ToggleExplanation(true);
                Debug.LogWarning("Player right of Start");
            }
        }
        else
        {
            Debug.LogWarning("Player never moved left");
        }
    }
    internal void TakeOff()
    {
        ///speed and physics are applied 
        anim.SetTrigger("fly");
        playerRB.isKinematic = false;
        playerRB.AddForce(jumpForce, ForceMode2D.Impulse);
        gameController.SwitchCamera();
        gameController.HideTrajectory();

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        audioSource.Stop();
        anim.SetTrigger("collided");
    }
    
    internal void ShowEndAnimation()
    {
        uIManager.gameEndButton.SetActive(true);
        int currentScore = gameController.GetComponentInChildren<JH_ScoreCalculator>().score;
        uIManager.ShowScore(currentScore);
        if(DataCollector.currentPlayer != null)
        {
            DataCollector.UpdateScore(currentScore);
        }
        else
        {
            Debug.LogError("No current player found, couldn't save score");
        }

        if (currentScore == 0)
        {
            anim.SetBool("overshot", true);
            anim.SetBool("success", false);
            anim.SetBool("lost", false);
            spectatorHandler.EndOfGameReaction(WinState.Neutral);
        }
        else if(currentScore > 0)
        {
            anim.SetBool("overshot", false);
            anim.SetBool("success", true);
            anim.SetBool("lost", false);
            spectatorHandler.EndOfGameReaction(WinState.Win);
        }
        else if (currentScore < 0)
        {
            anim.SetBool("overshot", false);
            anim.SetBool("success", false);
            anim.SetBool("lost", true);
            spectatorHandler.EndOfGameReaction(WinState.Loss);
        }
        else
        {
            anim.SetBool("overshot", true);
            anim.SetBool("success", false);
            anim.SetBool("lost", false);
            spectatorHandler.EndOfGameReaction(WinState.Neutral);
        }
    }

    private void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }

    private void TriggerSound (JH_SoundList soundList)
    {
        switch (soundList)
        {
            case JH_SoundList.Step:
                PlaySound( audioClips[0]);
                break;
            case JH_SoundList.Step2:
                PlaySound( audioClips[1]);
                break;
            case JH_SoundList.Flight:
                PlaySound( audioClips[2]);
                break;
            default:
                return;
        }
    }

    ///called from animation event
    private void CallWallCollisionChange()
    {
        gameController.ChangeCollision(gameController.wallChildren, 9);
    }

    //TEMPORARY UI CALLS FOR TESTING
    public void ChangeForceValue(string value)
    {
       jumpPowerMod = Single.Parse(value);
    }
    public void ChangeForceXValue(string value)
    {
        jumpPowerModX = Single.Parse(value);
    }
    public void ChangeForceYValue(string value)
    {
        jumpPowerModY = Single.Parse(value);
    }
}