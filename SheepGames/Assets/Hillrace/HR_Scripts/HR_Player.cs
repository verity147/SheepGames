using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HR_Player : MonoBehaviour
{
    public float runSpeed = 10f;
    public float aircontrolSpeed = 10f;
    public float jumpforce = 10f;
    public float maxYspeed = 10f;
    public float maxJumpHeight = 10f;
    public float jumpGravity;

    internal bool isGrounded;

    private Rigidbody2D myRigidbody;
    private bool lookRight = true;
    private float startY;
    private float normalGravity;
    private bool jump = false;

    private void Awake()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        normalGravity = myRigidbody.gravityScale;
    }

    private void FixedUpdate()
    {
        if (Input.GetAxisRaw("Horizontal") > Mathf.Abs(0.01f))
        {
            Move();
        }

        if (lookRight == false && Input.GetAxisRaw("Horizontal") > 0)
        {
            Flip();
        }
        else if (lookRight == true && Input.GetAxisRaw("Horizontal") < 0)
        {
            Flip();
        }
    }

    private void Move()
    {
        //StartCoroutine(WalkThenRun(moveInput));
        if (isGrounded)
        {
            myRigidbody.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * runSpeed, myRigidbody.velocity.y);
        }
        else if (!isGrounded)
        {
            myRigidbody.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * aircontrolSpeed, myRigidbody.velocity.y);
        }
    }

    private void Update()
    {
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
                myRigidbody.velocity = Vector2.up * jumpforce;
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

  
    private IEnumerator WalkThenRun(float moveInput)
    {

        yield return null;
    }

    private void Flip()
    {
        lookRight = !lookRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
}
