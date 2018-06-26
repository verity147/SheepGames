using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JH_ObstacleSheep : MonoBehaviour {

    private Animator anim;
    private Rigidbody2D rigidb;


    private void Awake()
    {
        anim = GetComponent<Animator>();
        rigidb = GetComponent<Rigidbody2D>();
        rigidb.isKinematic = true;
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
    }

}
