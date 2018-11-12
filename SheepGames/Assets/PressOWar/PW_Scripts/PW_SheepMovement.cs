using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PW_SheepMovement : MonoBehaviour {

    public float pushForce;
    public float enemyStrengthMod;
    public float enemyWaitTimeInSec;
    public float enemyRhythmMod;
    public bool enemy;

    private Animator anim;
    private Rigidbody2D rBody;
    private bool gameIsOn = false;

    private void Awake()
    {
        rBody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        anim.SetFloat("Movement", rBody.velocity.x);
    }

    ///called from InputManager
    public void StartGame()
    {
        gameIsOn = true;
        anim.SetTrigger("GameStart");
        if (enemy)
        {
            StartCoroutine(EnemyMovement());
        }
    }

    ///called from InputManager
    public void StopGame()
    {
        gameIsOn = false;
        StopAllCoroutines();
        anim.SetTrigger("GameEnd");
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
