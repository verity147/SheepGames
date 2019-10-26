using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class HR_Player : MonoBehaviour
{
    public float maxRunSpeed = 10f;
    public float swimSpeed = 4f;
    public float accelTime = 2f;        ///lerpTime
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
    } ///resets fallDuration on change
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
    private float drinkTime = 0f;
    private float maxDrinkTime = 2f;

    private void Awake()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
    }

    private void Start()
    {
        normalGravity = myRigidbody.gravityScale;
        jumpHeight = maxJumpHeight;
    }

    private void Update()
    {
        HandleDrinkingState();

        ///prevents movement while drinking or stunned
        HandleMovementInput();

        HandleJumping();

        CalculateFallTime();

        CheckStunCondition();

        CapMaxYSpeed();

        HandleDrinkingInput();

        SetAnimatorParameters();
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

    private void SetAnimatorParameters()
    {
        myAnimator.SetFloat("hSpeed", Mathf.Abs(myRigidbody.velocity.x));
        myAnimator.SetFloat("vSpeed", myRigidbody.velocity.y);
        myAnimator.SetBool("grounded", IsGrounded);
        myAnimator.SetBool("swimming", isSwimming);
    }

    
    private void CapMaxYSpeed()
    {
        if (myRigidbody.velocity.y > maxYspeed)
        {
            myRigidbody.velocity = new Vector2(myRigidbody.velocity.x, maxYspeed);
        }
    }

    private void CheckStunCondition()
    {
        if (fallDuration >= fallStunTime)
        {
            myAnimator.SetBool("fall", true);
            fallDuration = 0f;
            stun = true;
        }
    }

    private void CalculateFallTime()
    {
        if (myRigidbody.velocity.y < 0 && !IsGrounded)
        {
            fallDuration += Time.deltaTime;
        }
    }

    private void HandleJumping()
    {
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
    }
  
    private void HandleDrinkingState()
    {
        if (drinking)
        {
            drinkTime += Time.deltaTime;
            if (drinkTime > maxDrinkTime)
            {
                drinkTime = maxDrinkTime;
            }
            float perc = drinkTime / maxDrinkTime;
            GetComponentInChildren<HR_PlayerCanvas>().drinkMeter.value = perc;//change
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
        //flip drinking meter here?
    }

    private void StunOver() ///called from anim event
    {
        stun = false;
        myAnimator.SetBool("fall", false);
    }

    private void StopPlayer() ///called from anim event
    {
        myRigidbody.velocity = Vector2.zero;
    }

}
