﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PW_SheepMovement : MonoBehaviour {

    public float pushForce;
    public float enemyStrengthMod;
    public float enemyWaitTimeInSec;
    public float enemyRhythmMod;
    public bool enemy;
    public float winPosX;
    public float losePosX;
    public float clashPosX;

    private Animator anim;
    private Rigidbody2D rBody;
    private bool gameIsOn = false;
    private PW_InputManager inputManager;

    internal Vector3 startPos;

    private void Awake()
    {
        rBody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        inputManager = FindObjectOfType<PW_InputManager>();
    }

    private void Start()
    {
        startPos = transform.position;
    }

    private void Update()
    {
        ///limit these checks to when the game is actually running
        if (!gameIsOn)
            return;
        if (!enemy)
        {
            anim.SetFloat("Movement", rBody.velocity.x);
        }
        else
        {
            anim.SetFloat("Movement", - rBody.velocity.x);
        }

        if (!enemy)
        {
            if (transform.position.x >= winPosX)
            {
                inputManager.EndGame(true);
            }
            if (transform.position.x <= losePosX)
            {
                inputManager.EndGame(false);
            }
        }
    }

    ///called from InputManager
    public void StartGame()
    {
        gameIsOn = true;
        anim.SetTrigger("GameStart");
        StartCoroutine(MoveToPoint(clashPosX));
        if (enemy)
        {
            StartCoroutine(EnemyMovement());
        }
    }

    ///called from InputManager
    public void StopGame(bool win)
    {
        gameIsOn = false;
        rBody.velocity = Vector2.zero;
        StopAllCoroutines();
        anim.SetBool("WinLose", win);
        anim.SetTrigger("GameEnd");
        if(enemy && !win)
        {
            gameObject.layer = 9;
            ///enemy needs to fly right until he sits in the lake
            StartCoroutine(MoveToPoint(losePosX + 3f));
        }
    }

    private IEnumerator MoveToPoint(float targetX)
    {
        float time = 0f;
        ///have to reach the goal a little before the animation ends for it to look good
        float clipLength = anim.GetCurrentAnimatorClipInfo(0)[0].clip.length - 0.15f;
        while (time < 1)
        {
            time += Time.deltaTime / clipLength;
            transform.position = Vector3.Lerp(startPos, new Vector3(targetX, transform.position.y, 0f), time);
            yield return null;
        }
    }
	
    internal void Push(float force)
    {
        Vector2 push = new Vector2(0f, 0f);

        ///make the player go right and give him his precision bonus multiplier
        if (!enemy)
        {
            ///positive numbers to go right
            force = Mathf.Abs(pushForce * force);
            push = new Vector2(force, 0);
            rBody.AddForce(push, ForceMode2D.Impulse);
        }
        else
        {
            push = new Vector2(force * -1f, 0);
            rBody.AddForce(push, ForceMode2D.Impulse);
        }
    }

    internal void Push(float force, float forceBonus)
    {
        Vector2 push = new Vector2(0f, 0f);
        force = Mathf.Abs(force);
        ///make the player go right and give him his precision bonus multiplier and boost bonus
        if (!enemy)
        {
            ///positive numbers to go right
            force = pushForce * force + forceBonus;
            push = new Vector2(force, 0);
            rBody.AddForce(push, ForceMode2D.Impulse);
        }
        else
        {
            push = new Vector2(force * -1f, 0);
            rBody.AddForce(push, ForceMode2D.Impulse);
        }
    }

    ///gives the enemy extra pushes when the player makes mistakes
    public void EnemyPushHelper()
    {
        Push(Random.Range(pushForce - enemyStrengthMod, pushForce + enemyStrengthMod));
    }

    IEnumerator EnemyMovement() {
        if (gameIsOn)
        {
            Push(Random.Range(pushForce - enemyStrengthMod, pushForce + enemyStrengthMod));
            float waitTime = Random.Range(enemyWaitTimeInSec - enemyRhythmMod, enemyWaitTimeInSec + enemyRhythmMod);
            yield return new WaitForSeconds(waitTime);//add animation length to waitTime?
            StartCoroutine(EnemyMovement());
        }
        yield return null;
    }
}
