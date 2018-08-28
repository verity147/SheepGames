using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PW_SheepMovement : MonoBehaviour {

    public float pushForce;
    public float enemyStrengthMod;
    public float enemyRhythmMod;
    public float enemyWaitTimeInSec;
    public bool enemy;

    private Rigidbody2D rBody;
    private bool gameIsOn = false;


    private void Awake()
    {
        rBody = GetComponent<Rigidbody2D>();        
    }

    public void StartGame()
    {
        gameIsOn = true;
        if (enemy)
        {
            StartCoroutine(EnemyMovement());
        }
    }

    public void StopGame()
    {
        gameIsOn = false;
        StopAllCoroutines();
    }
	
    internal void Push(float force)
    {
        ///make the player go left and give him his precision bonus multiplier
        if (!enemy)
        {
            force = (pushForce * force) * -1f;
        }
        Vector2 push = new Vector2(force, 0);
        rBody.AddForce(push, ForceMode2D.Impulse);
    }

    internal void Push(float force, float forceBonus)
    {
        ///make the player go left and give him his precision bonus multiplier and boost bonus
        if (!enemy)
        {
            force = (pushForce * force + forceBonus) * -1f;
        }
        Vector2 push = new Vector2(force, 0);
        rBody.AddForce(push, ForceMode2D.Impulse);
    }

    IEnumerator EnemyMovement() {
        if (gameIsOn)
        {
            Push(Random.Range(pushForce - enemyStrengthMod, pushForce + enemyStrengthMod));
            float waitTime = Random.Range(enemyWaitTimeInSec - enemyRhythmMod, enemyWaitTimeInSec + enemyRhythmMod);
            yield return new WaitForSeconds(waitTime);
            StartCoroutine(EnemyMovement());
        }
        yield return null;
    }
}
