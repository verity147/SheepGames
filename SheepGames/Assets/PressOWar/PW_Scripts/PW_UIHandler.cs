using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PW_UIHandler : MonoBehaviour {

    public bool automaticStartGame = false;
    public float countdownWaitSeconds = 1.2f;
    [Space]
    public TMP_Text countDown;
    public GameObject scoreTextObject;
    public TMP_Text score;
    public GameObject drummer;
    public GameObject endOfGameMenu;

    private PW_InputManager inputManager;
    private LocalizationManager localizationManager;
    private HighscoreHandler highscoreHandler;
    private PopulateHighscore populateHighscore;
    private int countdownCounter = 3;

    #region SCORE LOCALIZATION KEYS
    private readonly string scoresEqualKey = "EqualScore_T";
    private readonly string noPreviousScoreKey = "FirstScore_T";
    private readonly string newScoreBetterKey = "newScoreBetter_T";
    private readonly string oldScoreBetterKey = "oldScoreBetter_T";
    #endregion

    private void Awake()
    {
        inputManager = FindObjectOfType<PW_InputManager>();
        localizationManager = FindObjectOfType<LocalizationManager>();
        highscoreHandler = FindObjectOfType<HighscoreHandler>();
        populateHighscore = GetComponentInChildren<PopulateHighscore>(true);
    }

    private void Start()
    {
        countDown.text = countdownCounter.ToString();
        if(automaticStartGame)
            StartCoroutine(Countdown());
    }

    private IEnumerator Countdown()
    {
        yield return new WaitForSeconds(countdownWaitSeconds);
        countdownCounter--;
        countDown.text = countdownCounter.ToString();
        if (countdownCounter < 1)
        {
            if(drummer != null)
            {
                drummer.GetComponent<Animator>().SetTrigger("drumroll");
            }
            countDown.text = "";
            inputManager.StartGame();
            yield break;
        }
        StartCoroutine(Countdown());
    }

    ///needed because you can't start a coroutine from a ui button 
    public void ExecuteGameStart()
    {
        StartCoroutine(Countdown());
    }

 
}
