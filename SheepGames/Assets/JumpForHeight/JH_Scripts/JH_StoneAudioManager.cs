using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JH_StoneAudioManager : MonoBehaviour {

    private AudioSource source;

    private void Awake()
    {
        source = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && !source.isPlaying)
        {
            source.PlayOneShot(source.clip);
        }
    }
}
