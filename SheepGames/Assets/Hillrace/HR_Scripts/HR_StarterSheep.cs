using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HR_StarterSheep : MonoBehaviour
{
    public TMP_Text countDownText;

    private HR_Gamemanager gamemanager;
    private Animator anim;
    private AudioSource audioSource;
    private int countHelper = 3;

    private void Awake()
    {
        gamemanager = GetComponentInParent<HR_Gamemanager>();
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    public void StartGame()
    {
        anim.SetTrigger("startGame");
    }

    public void Countdown( string counter)
    {
        countDownText.text = counter;
        if (countHelper < 1)
        {
            gamemanager.StartPauseGame(true);
        }
        countHelper--;
    }

    private void PlayAudio()
    {
        audioSource.Play();
    }
}
