using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JH_ObstacleSheep : MonoBehaviour {

    public Vector3 goLeftPos;
    public float jumpStrength;

    private Animator anim;
    private Rigidbody2D rigidb;
    private bool stopGoingLeft = false;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        rigidb = GetComponent<Rigidbody2D>();
        rigidb.isKinematic = true;
    }

    private void Update()
    {
        if (stopGoingLeft == false)
        {
            if (transform.position != goLeftPos)
            {
                print("goingLeft");
                float step = anim.GetCurrentAnimatorClipInfo(0).Length/Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, goLeftPos, step);
            }else if (transform.position == goLeftPos)
            {
                rigidb.isKinematic = false;
                stopGoingLeft = true;
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            print("hit playertrigger");
            //Vector3 currentPos = transform.position;
            rigidb.isKinematic = false;
            //anim.applyRootMotion = false;
            //transform.position = currentPos;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            print("hit player");
            anim.SetTrigger("collision");
        }
        if (collision.gameObject.tag == "Ground")
        {
            print("touchGround");
            anim.SetTrigger("bounce");
        }
    }

    public void Jump()
    {
        print("addForce");
        rigidb.AddForce(new Vector2(0, jumpStrength), ForceMode2D.Impulse);
    }
}
