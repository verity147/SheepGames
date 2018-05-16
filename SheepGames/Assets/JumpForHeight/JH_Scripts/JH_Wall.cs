using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JH_Wall : MonoBehaviour {

    public AudioClip clip;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            GetComponent<AudioSource>().PlayOneShot(clip);
        }
    }
}
