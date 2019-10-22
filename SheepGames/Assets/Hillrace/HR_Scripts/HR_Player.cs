using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Anima2D;

public class HR_Player : MonoBehaviour
{
    public float maxRunSpeed = 10f;
    public float swimSpeed = 4f;
    public float accelTime = 2f;        //lerpTime
    public float aircontrolSpeed = 10f;
    public float jumpforce = 10f;
    public float maxYspeed = 10f;
    public float maxJumpHeight = 10f;
    public float maxSwimJumpHeight = 10f;
    public float jumpGravity = 1f;
    public float swimJumpGravity = 1f;
    public float fallStunTime = 0.5f;

    private bool isGrounded;
    public bool IsGrounded
    {
        get { return isGrounded;  }
        set
        {
            if(value == isGrounded)
            {
                return;
            }
            isGrounded = value;
            if (isGrounded)
            {
                fallDuration = 0f;
            }
        }
    }
    internal bool isSwimming;
    internal bool drinkingAllowed = false;
    internal bool drinking = false;

    private Rigidbody2D myRigidbody;
    private Animator myAnimator;
    private bool lookRight = true;
    private bool stun = false;
    private bool jump = false;
    private float startY;
    private float normalGravity;
    private float currentLerpTime = 0f;
    private float lerpStep;
    private float jumpHeight;
    private float fallDuration = 0f;

    private void Awake()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        normalGravity = myRigidbody.gravityScale;
        //face change
        //GetComponentInChildren<SpriteMeshAnimation>().frame = 1;
    }

    private void Start()
    {
        jumpHeight = maxJumpHeight;
    }

    private void Update()
    {
        if (drinking)
        {
            myRigidbody.velocity = Vector2.zero;
        }

        ///prevent movement while drinking
        if (!drinking && !stun)
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

            if (Input.GetButtonDown("Jump") && IsGrounded)
            {
                myAnimator.SetTrigger("Jump");
                startY = transform.position.y;
                myRigidbody.gravityScale = isSwimming ? swimJumpGravity : jumpGravity;
                jumpHeight = isSwimming ? maxSwimJumpHeight : maxJumpHeight;
                jump = true;
            }

        }

        if (jump)
        {
            if (Input.GetButton("Jump") && transform.position.y <= startY + jumpHeight)
            {
                myRigidbody.velocity = new Vector2(myRigidbody.velocity.x * 0.8f, jumpforce);
            }

            if (transform.position.y >= startY + jumpHeight || Input.GetButtonUp("Jump"))
            {
                jump = false;
                myRigidbody.gravityScale = normalGravity;
            }
        }

        if(myRigidbody.velocity.y < 0 && !IsGrounded)
        {
            fallDuration += Time.deltaTime;
        }

        if (fallDuration >= fallStunTime)
        {
            myAnimator.SetBool("fall", true);
            fallDuration = 0f;
            stun = true;
            print("stun");
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
        myAnimator.SetBool("grounded", IsGrounded);
        myAnimator.SetBool("swimming", isSwimming);
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


    private void ManageRunSpeed()
    {
        currentLerpTime += Time.deltaTime;
        if (currentLerpTime < accelTime)
        {
            float runSpeed;
            lerpStep = currentLerpTime / accelTime;
            lerpStep = Mathf.Sin(lerpStep * Mathf.PI * 0.5f);
            runSpeed = Mathf.Lerp(0f, isSwimming ? swimSpeed : maxRunSpeed, lerpStep);
            Move(runSpeed);
        }
        else
        {
            Move(isSwimming ? swimSpeed : maxRunSpeed);
        }
    }

    private void Drink()
    {
        throw new NotImplementedException();
    }

    private void Move(float runSpeed)
    {
        if (IsGrounded)
        {
            myRigidbody.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * runSpeed, myRigidbody.velocity.y);
        }
        else if (!IsGrounded)
        {
            myRigidbody.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * aircontrolSpeed, myRigidbody.velocity.y);
        }
    }

    private void Flip()
    {
        if (!drinking && !stun)
        {
            lookRight = !lookRight;
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }
    }

    private void StunOver()
    {
        stun = false;
        myAnimator.SetBool("fall", false);
    }
}
