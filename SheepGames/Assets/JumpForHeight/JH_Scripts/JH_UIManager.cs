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
    public SceneHandler sceneHandler;
    public GameObject retryButton;
    public GameObject showScore;
    public GameObject gameEndButton;
    public TMP_Text tutorialMenuText;

    private LocalizationManager localizationManager;
    private JH_ScoreCalculator scoreCalculator;
    private int tries = 0;
    private readonly int maxTries = 3;

    private readonly string scoreTextKey = "ShowGameScore_T";
    private readonly string tutorialTextKey = "JH_Tutorial_T";

    private void Awake()
    {
        localizationManager = FindObjectOfType<LocalizationManager>();
        scoreCalculator = FindObjectOfType<JH_ScoreCalculator>();
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
}