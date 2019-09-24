using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HR_Player : MonoBehaviour
{
    public float maxRunSpeed = 10f;
    public float accelTime = 2f;        //lerpTime
    public float aircontrolSpeed = 10f;
    public float jumpforce = 10f;
    public float maxYspeed = 10f;
    public float maxJumpHeight = 10f;
    public float jumpGravity;

    internal bool isGrounded;

    private Rigidbody2D myRigidbody;
    private Animator myAnimator;
    private bool lookRight = true;
    private float startY;
    private float normalGravity;
    private bool jump = false;
    private float currentLerpTime = 0f;
    private float lerpStep;

    private void Awake()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        normalGravity = myRigidbody.gravityScale;
    }

    private void Update()
    {
        if (Input.GetButtonDown("Left") || Input.GetButtonDown("Right"))
        {
            currentLerpTime = 0f;
        }

        if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) > 0.01f)
        {
            currentLerpTime += Time.deltaTime;
            if (currentLerpTime < accelTime)
            {
                float runSpeed;
                lerpStep = currentLerpTime / accelTime;
                ///smoother curves for the speed
                //lerpStep = lerpStep * lerpStep * lerpStep * (lerpStep * (6f * lerpStep - 15f) + 10f);
                //lerpStep = lerpStep * lerpStep * (3f - 2f * lerpStep);
                lerpStep = Mathf.Sin(lerpStep * Mathf.PI * 0.5f);
                runSpeed = Mathf.Lerp(0f, maxRunSpeed, lerpStep);
                Move(runSpeed);
            }
            else
            {
                Move(maxRunSpeed);
            }
            myAnimator.SetFloat("hSpeed", runSpeed);
        }

        //stop the player
        if(Input.GetButtonUp("Left") || Input.GetButtonUp("Right") || Input.GetButton("Left") && Input.GetButton("Right"))
        {
            myRigidbody.velocity = new Vector2(0f, myRigidbody.velocity.y);
        }

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            startY = transform.position.y;
            myRigidbody.gravityScale = jumpGravity;
            jump = true;
        }

        if (jump)
        {
            if (Input.GetButton("Jump") && transform.position.y <= startY + maxJumpHeight)
            {
                myRigidbody.velocity = new Vector2(myRigidbody.velocity.x * 0.8f, jumpforce);
            }

            if (transform.position.y >= startY + maxJumpHeight || Input.GetButtonUp("Jump"))
            {
                jump = false;
                myRigidbody.gravityScale = normalGravity;
            }
        }

        if (myRigidbody.velocity.y > maxYspeed)
        {
            myRigidbody.velocity = new Vector2(myRigidbody.velocity.x, maxYspeed);
        }

    }

    private void FixedUpdate()
    {

        if (lookRight == false && Input.GetAxisRaw("Horizontal") > 0)
        {
            Flip();
        }
        else if (lookRight == true && Input.GetAxisRaw("Horizontal") < 0)
        {
            Flip();
        }
    }

    private void Move(float runSpeed)
    {
        if (isGrounded)
        {
            myRigidbody.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * runSpeed, myRigidbody.velocity.y);
        }
        else if (!isGrounded)
        {
            myRigidbody.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * aircontrolSpeed, myRigidbody.velocity.y);
        }
    }

     private void Flip()
    {
        lookRight = !lookRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
}
