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
    public TMP_Text tutorialMenuText;

    private LocalizationManager localizationManager;
    private PopulateHighscore populateHighscore;
    private JH_ScoreCalculator scoreCalculator;
    private int tries = 0;
    private int maxTries = 3;

    #region LOCALIZATION KEYS
    private readonly string scoresEqualKey = "EqualScore_T";
    private readonly string noPreviousScoreKey = "FirstScore_T";
    private readonly string newScoreBetterKey = "newScoreBetter_T";
    private readonly string oldScoreBetterKey = "oldScoreBetter_T";
    private readonly string scoreTextKey = "ShowGameScore_T";
    private readonly string tutorialTextKey = "JH_Tutorial_T";
    #endregion

    private void Awake()
    {
        localizationManager = FindObjectOfType<LocalizationManager>();
        scoreCalculator = FindObjectOfType<JH_ScoreCalculator>();
        populateHighscore = GetComponentInChildren<PopulateHighscore> (true);
    }
    private void Start()
    {
        ///fills Tutorial with accurate Information about score points
        if (SceneHandler.GetSceneName() == "JH_GameLV_01")
        {
            string tutorialText;
            tutorialText = string.Format(localizationManager.GetLocalizedText(tutorialTextKey), scoreCalculator.winPointBonus.ToString(), scoreCalculator.stonePointPenalty.ToString());
            tutorialMenuText.text = tutorialText;
        }
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
        populateHighscore.NewLevelScore(SceneHandler.GetSceneName());

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