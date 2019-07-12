using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HR_Player : MonoBehaviour
{
    public float runSpeed = 10f;
    public float aircontrolSpeed = 10f;
    public float jumpforce = 10f;
    public float maxYspeed = 10f;

    internal bool isGrounded;

    private Rigidbody2D myRigidbody;
    private float moveInput;
    private bool lookRight = true;

    private void Awake()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        moveInput = Input.GetAxisRaw("Horizontal");
        if (isGrounded)
        {
            myRigidbody.velocity = new Vector2(moveInput * runSpeed, myRigidbody.velocity.y);
        }
        else if (!isGrounded)
        {
            myRigidbody.velocity = new Vector2(moveInput * aircontrolSpeed, myRigidbody.velocity.y);
        }

        if (lookRight == false && moveInput > 0)
        {
            Flip();
        }else if(lookRight == true && moveInput < 0)
        {
            Flip();
        }
    }

    private void Update()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            myRigidbody.velocity = Vector2.up * jumpforce;
        }

        if (myRigidbody.velocity.y > maxYspeed)
        {
            myRigidbody.velocity = new Vector2(myRigidbody.velocity.x, maxYspeed);
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
