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
    public GameObject scoreTextObject_T;
    public SceneHandler sceneHandler;
    public GameObject retryButton;
    public GameObject endOfGameMenu;
    public GameObject endOfGameMenu_T;
    public GameObject showScore;
    public GameObject gameEndButton;
    public TMP_Text tutorialMenuText;

    private LocalizationManager localizationManager;
    private HighscoreHandler highscoreHandler;
    private PopulateHighscore populateHighscore;
    private JH_ScoreCalculator scoreCalculator;
    private int tries = 0;
    private readonly int maxTries = 3;

    private readonly string scoreTextKey = "ShowGameScore_T";
    private readonly string tutorialTextKey = "JH_Tutorial_T";

    private void Awake()
    {
        localizationManager = FindObjectOfType<LocalizationManager>();
        highscoreHandler = FindObjectOfType<HighscoreHandler>();
        scoreCalculator = FindObjectOfType<JH_ScoreCalculator>();
        populateHighscore = GetComponentInChildren<PopulateHighscore> (true);
    }
    private void Start()
    {
        ///fills Tutorial with accurate Information about score points
        if (SceneHandler.GetSceneName() == "JH_GameLV_01")
        {
            string tutorialText = "Text not found.";
            tutorialText = string.Format(localizationManager.GetLocalizedText(tutorialTextKey), 
                                            scoreCalculator.winPointBonus.ToString(), 
                                            scoreCalculator.stonePointPenalty.ToString());
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
        if (TournamentTracker.IsTournamentRunning())
        {
            endOfGameMenu_T.SetActive(true);
            populateHighscore = endOfGameMenu_T.GetComponentInChildren<PopulateHighscore>();
            scoreTextObject_T.GetComponent<TMP_Text>().text = highscoreHandler.GetHighscoreText();
        }
        else
        {
            endOfGameMenu.SetActive(true);
            populateHighscore = endOfGameMenu.GetComponentInChildren<PopulateHighscore>();
            scoreTextObject.GetComponent<TMP_Text>().text = highscoreHandler.GetHighscoreText();
        }
        populateHighscore.NewGrid();
        populateHighscore.NewLevelScore(SceneHandler.GetSceneName());
    }
}