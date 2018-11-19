using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction
    {
        Left = 0,
        Right = 1,
        Up = 2,
        Down = 3
    }

public class PW_Direction : MonoBehaviour {

    public float moveSpeed = 5f;

    public Direction direction;

    private PW_InputManager inputManager;

    private void Awake()
    {
        inputManager = FindObjectOfType<PW_InputManager>();
    }

    private void Update()
    {
        if (!inputManager.gameIsRunning)
        {
            Disable();
            gameObject.SetActive(false);
        }else
            transform.position += Vector3.down * Time.deltaTime * moveSpeed;        
    }

    internal void Disable()
    {
        //do an effect;
        gameObject.SetActive(false);
    }
}
