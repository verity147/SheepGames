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
    private Vector3 lastPos = Vector3.zero;
    private Vector2 startPos;

    private void Awake()
    {
        projControl = FindObjectOfType<JH_ProjectileControl>();
        gameController = FindObjectOfType<JH_GameController>();
        anim = GetComponent<Animator>();
        playerRB = GetComponent<Rigidbody2D>();
        playerColl = GetComponent <CompositeCollider2D>();
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
        }
        if (projControl.isPressed)
        {
            speed = (lastPos - transform.position).magnitude;
            anim.SetFloat("moveSpeed", speed);
            //lastMousePos = currentMousePos;
            lastPos = transform.position;
        }
        ///run up after mouse is released
        if (timeForRunUp && transform.position.x < startPos.x)
        {
            anim.SetTrigger("playerRelease");
            float clipLength = anim.GetCurrentAnimatorClipInfo(0)[0].clip.length;
            print(anim.GetCurrentAnimatorClipInfo(0)[0].clip.name);
            playerColl.enabled = true;
            ///x position can put playerBody backwards if runwayDist is too short
            if (projControl.transform.position.x >= transform.position.x)
            playerRB.MovePosition(new Vector2(projControl.transform.position.x, transform.position.y));
        }
    }
    private void MovePlayerToStart()
    {
       float distance = projControl.transform.position.x - transform.position.x;
    }
    internal void TakeOff()
    {
        ///speed and physics are applied 
        anim.SetTrigger("fly");
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
   