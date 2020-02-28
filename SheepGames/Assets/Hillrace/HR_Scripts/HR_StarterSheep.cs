using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HR_StarterSheep : MonoBehaviour
{
    public TMP_Text countDownText;
    public int counterSteps;

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
        countHelper = counterSteps;
    }

    public void Countdown( string counter)
    {
        countHelper--;
        countDownText.text = counter;
        if (countHelper < 1)
        {
            gamemanager.StartPauseGame(true);
        }
    }

    private void PlayAudio()
    {
        audioSource.Play();
    }
}
