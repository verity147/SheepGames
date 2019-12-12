using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class HR_Player : MonoBehaviour
{
    internal enum Jumpstate { normal, boosted, spring, swimming }

    public float maxRunSpeed = 10f;
    public float mudRunSpeed = 1f;
    public float swimSpeed = 4f;
    public float accelTime = 2f;        ///lerpTime
    public float aircontrolSpeed = 10f;
    public float jumpforce = 10f;
    public float jumpBoost = 10f;
    public float maxYspeed = 10f;
    public float standardJumpHeight = 10f;
    public float swimJumpHeight = 10f;
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
           
        }
    } 
    internal bool inMud;
    internal bool isSwimming;
    internal bool drinkingAllowed = false;
    internal bool drinking = false;

    private bool jump = false;
    private Rigidbody2D myRigidbody;
    private Animator myAnimator;
    private HR_PlayerCanvas playerCanvas;
    private bool lookRight = true;
    public bool Stunned { get; set; } = false;
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
        playerCanvas = GetComponentInChildren<HR_PlayerCanvas>();
    }

    private void Start()
    {
        normalGravity = myRigidbody.gravityScale;
        jumpHeight = standardJumpHeight;
    }    

    private void Update()
    {
        HandleDrinkingState();

        CalculateFallTime();

        HandleDrinkingInput();

        SetAnimatorParameters();

        HandleMovementInput();
    }

    private void FixedUpdate()
    {
        ///prevents movement while drinking or stunned

        CapMaxYSpeed();

        if (Stunned)
        {
            myRigidbody.velocity = new Vector2(0f, myRigidbody.velocity.y);
        }

        if (lookRight == false && Input.GetAxisRaw("Horizontal") > 0)
        {
            Flip();
        }
        else if (lookRight == true && Input.GetAxisRaw("Horizontal") < 0)
        {
            Flip();
        }
        if (jump)
        {
            myRigidbody.velocity = new Vector2(myRigidbody.velocity.x * 0.8f, jumpforce);
        }        
    }

    private void SetAnimatorParameters()
    {
        myAnimator.SetFloat("hSpeed", Mathf.Abs(myRigidbody.velocity.x));
        myAnimator.SetFloat("vSpeed", myRigidbody.velocity.y);
        myAnimator.SetBool("grounded", IsGrounded);
        myAnimator.SetBool("swimming", isSwimming);
        myAnimator.SetBool("inMud", inMud);
    }
    
    private void CapMaxYSpeed()
    {
        if (myRigidbody.velocity.y > maxYspeed)
        {
            myRigidbody.velocity = new Vector2(myRigidbody.velocity.x, maxYspeed);
        }
    }

    internal void PrepareJump(Jumpstate jumpstate)
    {
        switch (jumpstate)
        {
            case Jumpstate.normal:
                jumpHeight = standardJumpHeight;
                break;
            case Jumpstate.boosted:
                jumpHeight = standardJumpHeight + jumpBoost;
                break;
            case Jumpstate.spring:
                jumpHeight = standardJumpHeight + (jumpBoost * 2f);
                break;
            case Jumpstate.swimming:
                jumpHeight = swimJumpHeight;
                break;
        }
        myRigidbody.gravityScale = isSwimming ? swimJumpGravity : jumpGravity;
        myAnimator.SetTrigger("Jump");
        startY = transform.position.y;
        jump = true;
    }

    internal void Stun(GameObject other)
    {
        if (other.layer == 4 || other.tag == "Spring") /// "water"
        {
            fallDuration = 0f;
            return;
        }
        else if (other.layer == 15) ///"Ground" layer
        {
            if (fallDuration > fallStunTime)
            {
                myAnimator.SetBool("stun", true);
                fallDuration = 0f;
                Stunned = true;        
            }
            fallDuration = 0;
        }
    }

    private void CalculateFallTime()
    {
        if (myRigidbody.velocity.y < 0 && !IsGrounded)
        {
            fallDuration += Time.deltaTime;
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
            playerCanvas.drinkMeter.value = drinkTime / maxDrinkTime;
        }
        if (!drinking && drinkTime > 0f)
        {
            drinkTime -= Time.deltaTime * 0.5f; ///timer goes down way too fast otherwise
            playerCanvas.drinkMeter.value = drinkTime / maxDrinkTime;
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
            runSpeed = Mathf.Lerp(0f, WhichMoveSpeed(), lerpStep);
            Move(runSpeed);
        }
        else
        {
            Move(WhichMoveSpeed());
        }
    }

    private float WhichMoveSpeed()
    {
        if (isSwimming)
        {
            return swimSpeed;
        }
        else if (inMud)
        {
            return mudRunSpeed;
        }
        else
        {
            return maxRunSpeed;
        }
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
        if (!drinking && !Stunned)
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
        Stunned = false;
        myAnimator.SetBool("stun", false);
    }

    private void StopPlayer() ///called from anim event
    {
        myRigidbody.velocity = Vector2.zero;
    }

}
