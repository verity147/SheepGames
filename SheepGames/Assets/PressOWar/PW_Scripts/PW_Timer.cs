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
    private PW_InputManager inputManager;
    public bool gameStarted = false;

    private void Awake()
    {
        inputManager = FindObjectOfType<PW_InputManager>();        
    }

    private void Start()
    {
        startPos = transform.position;
        stateDuration = turnTimeInSec / sprites.Length;
    }

    private void SetUpTimer()
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

        startTime += Time.deltaTime;

        if (startTime >= turnTimeInSec)
        {
            StopAllCoroutines();

            print("Done");
        }        
    }

    private IEnumerator TimerSpriteChange()
    {
        currentState++;
        sRenderer.sprite = sprites[currentState];
        yield return new WaitForSeconds(stateDuration);
        StartCoroutine(TimerSpriteChange());
    }

    internal void ResetTimer()
    {
        transform.position = startPos;
        sRenderer.sprite = sprites[0];
        startTime = 0f;
    }

}
