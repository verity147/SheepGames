using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

///attached to Canvas
public class JH_UIManager : MonoBehaviour
{
    public GameObject speechBubble;
    public GameObject scoreTextObject;
    public SceneHandler sceneHandler;
    public GameObject retryButton;
    public GameObject endOfGameMenu;
    public GameObject showScore;
    public GameObject gameEndButton;

    private LocalizationManager localizationManager;
    private PopulateHighscore populateHighscore;
    private int tries = 0;
    private int maxTries = 3;

    #region LOCALIZATION KEYS
    private string scoresEqualKey = "EqualScore_T";
    private string noPreviousScoreKey = "FirstScore_T";
    private string newScoreBetterKey = "newScoreBetter_T";
    private string oldScoreBetterKey = "oldScoreBetter_T";
    private string scoreTextKey = "ShowGameScore_T";
    #endregion

    private void Awake()
    {
        localizationManager = FindObjectOfType<LocalizationManager>();
        populateHighscore = GetComponentInChildren<PopulateHighscore> (true);
    }

    internal void ToggleExplanation(bool active)
    {
        speechBubble.SetActive(active);
    }

    internal void CountTries()
    {
        tries++;
        if (tries >= maxTries)
        {
            retryButton.SetActive(false);
            //DataCollector.CalculateTotalScore();
        }
    }

    internal void ShowScore(int score)
    {
        showScore.SetActive(true);
        string scoreText;
        scoreText = string.Format(localizationManager.GetLocalizedText(scoreTextKey), score.ToString());
        showScore.GetComponent<TMP_Text>().text = scoreText;
    }

    public void BuildLevelEndMenu()
    {
        endOfGameMenu.SetActive(true);
        int oldScore = DataCollector.oldScore;
        int newScore = DataCollector.currentScore;
        string bestScoreText;

        populateHighscore.NewGrid();
        populateHighscore.NewLevelScore(sceneHandler.GetSceneName());

        if(oldScore == newScore)
        {
            bestScoreText = string.Format(localizationManager.GetLocalizedText(scoresEqualKey), oldScore.ToString(), newScore.ToString());
            scoreTextObject.GetComponent<TMP_Text>().text = bestScoreText;
        }
        else if(oldScore == -1000000)
        {
            bestScoreText = string.Format(localizationManager.GetLocalizedText(noPreviousScoreKey), newScore.ToString());
            scoreTextObject.GetComponent<TMP_Text>().text = bestScoreText;
        }
        else if(oldScore < newScore)
        {
            bestScoreText = string.Format(localizationManager.GetLocalizedText(newScoreBetterKey), oldScore.ToString(), newScore.ToString());
            scoreTextObject.GetComponent<TMP_Text>().text = bestScoreText;
        }
        else if (oldScore > newScore)
        {
            bestScoreText = string.Format(localizationManager.GetLocalizedText(oldScoreBetterKey), oldScore.ToString(), newScore.ToString());
            scoreTextObject.GetComponent<TMP_Text>().text = bestScoreText;
        }

    }
}