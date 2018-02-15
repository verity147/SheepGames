﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JH_PlayerBody : MonoBehaviour
{

    public float maxRunwayDist;
    ///has to be same length as RunUp animation clip
    public float runUpDuration;
    public float jumpPowerMod = 1f;
    public float resetDist = 0.5f;
    public float moveSpeed = 10f;

    private JH_GameController gameController;
    internal Animator anim;
    private Rigidbody2D playerRB;
    private Collider2D playerColl;

    private float speed;
    private float maxRunLeft;

    internal bool timeForRunUp = false;
    private bool hasJumped = false;

    internal Vector2 flightVel;
    private Vector2 lastMousePos = Vector2.zero;
    private Vector3 lastPos = Vector3.zero;
    private Vector2 startPos;
    private Vector2 currentMousePos = Vector2.zero;
    internal Vector2 jumpForce = Vector2.zero;
    private float movedDist;

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
            gameController.drawNow = true;
        }
    }


    private void OnMouseUp()
    {
        gameController.drawNow = false;
        if (movedDist > resetDist)
        {
            hasJumped = true;
            CalculateJumpForce();
            MovePlayerToStart();            
        }
        else
        {
            gameController.SpawnNewPlayer();
        }
    }

    private void CalculateJumpForce()
    {
        ///take mouse angle relative to the x-axis
        float mouseAngle = Mathf.Atan2(currentMousePos.y, currentMousePos.x);
        ///translate angle into vector
        Vector2 jumpDirection = new Vector2(Mathf.Abs((Mathf.Cos(mouseAngle))), Mathf.Abs((Mathf.Sin(mouseAngle))));
        ///multiply with player distance from start and power modificator
        jumpForce = jumpDirection * (movedDist * 10f) * jumpPowerMod;
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
        print(jumpForce);
        playerRB.AddForce(jumpForce, ForceMode2D.Impulse);
        gameController.SwitchCamera();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        anim.SetTrigger("collided");
    }


    internal void ShowEndAnimation()
    {
        int currentScore = gameController.GetComponentInChildren<JH_ScoreCalculator>().score;

        if(currentScore == 0)
        {
            anim.SetBool("overshot", true);
            anim.SetBool("success", false);
            anim.SetBool("lost", false);
        }
        else if(currentScore > 0)
        {
            anim.SetBool("overshot", false);
            anim.SetBool("success", true);
            anim.SetBool("lost", false);
        }
        else if (currentScore < 0)
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
    ///called from animation event
    private void CallWallCollisionChange()
    {
        gameController.ChangeCollision(gameController.wallChildren, 9);
    }
}