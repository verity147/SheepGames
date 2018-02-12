using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JH_PlayerBody : MonoBehaviour
{

    public float maxRunwayDist;
    public float runUpDuration;
    public float jumpPowerMod = 1f;

    private JH_GameController gameController;
    internal Animator anim;
    private Rigidbody2D playerRB;
    private Collider2D playerColl;

    private float speed;
    private float maxRunLeft;
    internal bool timeForRunUp = false;
    internal Vector2 flightVel;

    private Vector2 lastMousePos = Vector2.zero;
    private Vector3 lastPos = Vector3.zero;
    private Vector2 startPos;
    private Vector2 currentMousePos = Vector2.zero;
    internal Vector2 jumpForce = Vector2.zero;

    private bool hasJumped = false;

    private void Awake()
    {
        gameController = FindObjectOfType<JH_GameController>();
        anim = GetComponent<Animator>();
        playerRB = GetComponent<Rigidbody2D>();
        playerColl = GetComponent <CompositeCollider2D>();
        playerColl.enabled = false;
        startPos = gameController.transform.position;
    }
    private void Start()
    {
        maxRunLeft = startPos.x - maxRunwayDist;
    }
    private void Update()
    {
        currentMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    private void OnMouseDrag()
    {
        if (!hasJumped)
        {
            if (currentMousePos.x <= startPos.x && currentMousePos.x > maxRunLeft)
            {
                playerRB.MovePosition(new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, transform.position.y));
                speed = (lastPos - transform.position).magnitude;
                anim.SetFloat("moveSpeed", speed);
                lastPos = transform.position;
            }
            jumpForce = (new Vector2(startPos.x, startPos.y) - new Vector2(currentMousePos.x, currentMousePos.y)) * jumpPowerMod;
            //gameController.DrawTrajectoryPoints(currentMousePos, jumpForce / playerRB.mass);
        }
    }


    private void OnMouseUp()
    {
        hasJumped = true;
        jumpForce = (new Vector2(startPos.x, startPos.y) - new Vector2(currentMousePos.x, currentMousePos.y)) * jumpPowerMod;
        MovePlayerToStart();
    }

    internal void MovePlayerToStart()
    {
        if (transform.position.x < startPos.x)
        {
            anim.SetTrigger("playerRelease");
            ///call gamecontroller to change layer
            gameController.ChangeCollision(gameController.playerParts, 8);
            ///x position can put playerBody backwards if runwayDist is too short
            if (gameController.transform.position.x >= transform.position.x)
            {
                float distance = gameController.transform.position.x - transform.position.x;
                float speed = distance / runUpDuration;
                playerRB.velocity = new Vector2(speed, 0f);
            }
            else
            {
                ///if player is somehow to the right of start, respawn
                gameController.SpawnNewPlayer();
                Debug.LogWarning("Player right of Start 1");

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
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        anim.SetTrigger("collided");
        //if (overshot) result = 0;
        //if (won) result = 1;
        //if (lost) result = 2;
        //if (collision.relativeVelocity.magnitude > 2) to adjust animation to force of impact

    }


    internal void ShowEndAnimation()
    {
        int currentScore = gameController.GetComponentInChildren<JH_ScoreCalculator>().score;

        if(currentScore == 0)
        {
            anim.SetBool("overshot", true);
            anim.SetBool("success", false);
            anim.SetBool("lost", false);
        }else if(currentScore > 0)
        {
            anim.SetBool("overshot", false);
            anim.SetBool("success", true);
            anim.SetBool("lost", false);
        }else if (currentScore < 0)
        {
            anim.SetBool("overshot", false);
            anim.SetBool("success", false);
            anim.SetBool("lost", true);
        }
        else
        {
            anim.SetBool("overshot", true);
            anim.SetBool("success", false);
            anim.SetBool("lost", false);
        }

    }

    private void CallWallCollisionChange()
    {
        gameController.ChangeCollision(gameController.wallChildren, 9);
    }
}