using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JH_ObstacleSheep : MonoBehaviour {

    public Vector3 goLeftPos;
    public float jumpStrength;
    public float startGameWait = 1f;
    public Transform groundCheck;
    public LayerMask groundLayerMask;
    public AnimationClip walkLeftClip;

    private Animator anim;
    private Rigidbody2D rigidb;
    private bool stopGoingLeft = true;
    private bool grounded;
    private float groundRadius = 0.2f;
    private float moveLeftSpeed;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        rigidb = GetComponent<Rigidbody2D>();
        rigidb.isKinematic = true;
    }

    private void Start()
    {
        StartCoroutine(WaitForBouncer());
        float moveLeftDist = transform.position.x - goLeftPos.x;
        moveLeftSpeed = moveLeftDist / walkLeftClip.length;
    }

    private IEnumerator WaitForBouncer()
    {
        yield return new WaitForSeconds(startGameWait);
        stopGoingLeft = false;
    }

    private void Update()
    {        
        if (stopGoingLeft == false)
        {
            anim.SetTrigger("walkLeft");
            if (transform.position != goLeftPos)
            {
                float step = moveLeftSpeed * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, goLeftPos, step);
            }else if (transform.position == goLeftPos)
            {
                rigidb.isKinematic = false;
                stopGoingLeft = true;
            }
        }
    }

    private void FixedUpdate()
    {
        grounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, groundLayerMask);
        anim.SetBool("grounded", grounded);
        anim.SetFloat("vertSpeed", rigidb.velocity.y);

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            print("hit player");
            anim.SetTrigger("collision");
            gameObject.layer = 16; //collide with ground only
        }
    }

    public void Jump()
    {
        rigidb.AddForce(new Vector2(0, jumpStrength), ForceMode2D.Impulse);
    }
}
