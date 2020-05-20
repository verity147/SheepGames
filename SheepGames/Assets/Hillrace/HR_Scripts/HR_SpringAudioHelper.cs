using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HR_SpringAudioHelper : MonoBehaviour
{
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayAudio()
    {
        audioSource.Play();
    }

}
