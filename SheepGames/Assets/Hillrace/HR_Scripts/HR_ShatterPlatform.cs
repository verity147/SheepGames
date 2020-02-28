using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HR_ShatterPlatform : MonoBehaviour
{
    private Animator anim;
    private Collider2D coll;
    internal HR_Player player;
    private HR_Groundcheck groundcheck;
    private AudioSource audioSource;

    public AudioClip crumbleAudio;
    public AudioClip rebuildAudio;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        coll = GetComponent<EdgeCollider2D>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        groundcheck = player.GetComponentInChildren<HR_Groundcheck>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == groundcheck.gameObject &&  
            !anim.GetCurrentAnimatorStateInfo(0).IsName("HR_ShatterPlatform"))
        {
            anim.SetTrigger("shatter");
        }
    }

    private void ToggleCollider()
    {
        coll.enabled = !coll.enabled;
    }

    private void PlayAudio(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }
}
