using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JH_PlayerBody : MonoBehaviour
{

    public float maxRunwayDist;

    private JH_ProjectileControl projControl;
    private JH_GameController gameController;
    internal Animator anim;
    private Rigidbody2D playerRB;
    private Collider2D playerColl;

    private float speed;
    private float maxRunLeft;
    internal bool timeForRunUp = false;

    private Vector2 lastMousePos = Vector2.zero;
    private Vector2 startPos;

    private void Awake()
    {
        projControl = FindObjectOfType<JH_ProjectileControl>();
        gameController = FindObjectOfType<JH_GameController>();
        anim = GetComponent<Animator>();
        playerRB = GetComponent<Rigidbody2D>();
        playerColl = GetComponent <Collider2D>();
        playerColl.enabled = false;
        startPos = transform.position;
    }
    private void Start()
    {
        maxRunLeft = startPos.x - maxRunwayDist;
    }
    private void Update()
    {
        Vector2 currentMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        ///if mousebutton is pressed on player and it's left of Start and 
        if (projControl.isPressed && (currentMousePos.x <= (startPos.x) && (currentMousePos.x > maxRunLeft)))
        {            
            playerRB.MovePosition(new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, transform.position.y));
            speed = (lastMousePos - currentMousePos).magnitude;
            anim.SetFloat("speed", speed);
            lastMousePos = currentMousePos;
        }else if(projControl.isPressed)
        {
            speed = 0f;
            anim.SetFloat("speed", speed);
        }
        ///run up after mouse is released
        if (timeForRunUp && transform.position.x < startPos.x)
        {
            speed = 1f;
            anim.SetFloat("speed", speed);
            playerColl.enabled = true;
            ///x position can put playerBody backwards if runwayDist is too short
            if (projControl.transform.position.x >= transform.position.x)
            playerRB.MovePosition(new Vector2(projControl.transform.position.x, transform.position.y));
        }
    }

    ///speed and physics are applied 
    internal void TakeOff()
    {
        speed = 0;
        anim.SetFloat("speed", speed);
        anim.SetBool("flyBlend", true);
        playerRB.isKinematic = false;
        playerRB.velocity = projControl.flightVel;
        gameController.SwitchCamera();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        anim.SetBool("flyBlend", false);//instead start collision animation here
        gameController.ChangeWallCollision(); //trigger with animation, when the collision animation is sufficiently far along
        //if (collision.relativeVelocity.magnitude > 2) to adjust animation to force of impact
    }

}
