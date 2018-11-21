using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PW_UIHandler : MonoBehaviour {

    public TMP_Text countDown;
    public TMP_Text score;

    private PW_InputManager inputManager;
    private int countdownCounter = 3;

    private void Awake()
    {
        inputManager = FindObjectOfType<PW_InputManager>();
    }

    private void Start()
    {
        countDown.text = countdownCounter.ToString();
        StartCoroutine(Countdown());
    }

    private IEnumerator Countdown()
    {
        yield return new WaitForSeconds(1.5f);
        countdownCounter--;
        countDown.text = countdownCounter.ToString();
        if (countdownCounter < 1)
        {
            countDown.text = "";
            inputManager.StartGame();
            yield break;
        }
        StartCoroutine(Countdown());
    }
}
