using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PW_Timer : MonoBehaviour {

    public Sprite[] sprites;
    public SpriteRenderer sRenderer;
    public float turnTimeInSec = 60f;
    public Vector3 targetPos = new Vector3(8.4f, 0.7f, 0);
    public float speed = 1f;

    private float startTime = 0f;
    private float stateDuration;
    private int currentState = 0;
    private Vector3 startPos;
    private bool gameStarted = false;

    private void Start()
    {
        sRenderer.sprite = sprites[0];
        startPos = transform.position;
        stateDuration = turnTimeInSec / (sprites.Length - 1);
    }

    internal void SetUpTimer()
    {
        gameStarted = true;
        StartCoroutine(TimerSpriteChange());
    }

    private void Update()
    {
        if(transform.position != targetPos && gameStarted)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
        }    
    }

    private IEnumerator TimerSpriteChange()
    {
        yield return new WaitForSeconds(stateDuration);
        currentState++;
        sRenderer.sprite = sprites[currentState];
        if(sRenderer.sprite == sprites[sprites.GetUpperBound(0)])
        {
            print(" Timer Done");
            //notify someone that the game is over
            yield break;
        }
        StartCoroutine(TimerSpriteChange());
    }

    internal void ResetTimer()
    {
        transform.position = startPos;
        sRenderer.sprite = sprites[0];
        startTime = 0f;
    }

}
