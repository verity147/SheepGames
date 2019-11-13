using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HR_ShatterPlatform : MonoBehaviour
{
    private Animator anim;
    private Collider2D coll;
    internal HR_Player player;
    private HR_Groundcheck groundcheck;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();
    }

    private void Start()
    {
        groundcheck = player.GetComponentInChildren<HR_Groundcheck>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == groundcheck.gameObject)
        {
            anim.SetTrigger("shatter");
        }
    }

    public void ToggleCollider()
    {
        coll.enabled = !coll.enabled;
    }
}
