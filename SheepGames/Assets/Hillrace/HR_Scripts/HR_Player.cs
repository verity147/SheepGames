using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Anima2D;

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
    internal bool drinkingAllowed = false;
    internal bool drinking = false;

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
        //face change
        //GetComponentInChildren<SpriteMeshAnimation>().frame = 1;
    }

    private void Update()
    {
        ///prevent movement while drinking
        if (!drinking)
        {
            if (Input.GetButtonDown("Left") || Input.GetButtonDown("Right"))
            {
                currentLerpTime = 0f;
            }

            if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) > 0.01f)
            {
                ManageRunSpeed();
            }

            ///stop the player
            if (Input.GetButtonUp("Left") || Input.GetButtonUp("Right") || Input.GetButton("Left") && Input.GetButton("Right"))
            {
                myRigidbody.velocity = new Vector2(0f, myRigidbody.velocity.y);
            }

            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                myAnimator.SetTrigger("Jump");
                startY = transform.position.y;
                myRigidbody.gravityScale = jumpGravity;
                jump = true;
            }
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

        if (Input.GetButtonDown("Down") && drinkingAllowed)
        {
            myAnimator.SetTrigger("drink");
            myAnimator.SetBool("drinking", true);
            drinking = true;

        }

        if (Input.GetButtonUp("Down"))
        {
            myAnimator.SetBool("drinking", false);
            drinking = false;
        }

        myAnimator.SetFloat("hSpeed", Mathf.Abs(myRigidbody.velocity.x));
        myAnimator.SetFloat("vSpeed", myRigidbody.velocity.y);
        myAnimator.SetBool("grounded", isGrounded);
    }

    private void ManageRunSpeed()
    {
        currentLerpTime += Time.deltaTime;
        if (currentLerpTime < accelTime)
        {
            float runSpeed;
            lerpStep = currentLerpTime / accelTime;
            lerpStep = Mathf.Sin(lerpStep * Mathf.PI * 0.5f);
            runSpeed = Mathf.Lerp(0f, maxRunSpeed, lerpStep);
            Move(runSpeed);
        }
        else
        {
            Move(maxRunSpeed);
        }
    }

    private void Drink()
    {
        throw new NotImplementedException();
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
