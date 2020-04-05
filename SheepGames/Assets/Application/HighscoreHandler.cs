using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighscoreHandler : MonoBehaviour
{

    #region LOCALIZATION KEYS
    private readonly string scoresEqualKey = "EqualScore_T";
    private readonly string noPreviousScoreKey = "FirstScore_T";
    private readonly string newScoreBetterKey = "newScoreBetter_T";
    private readonly string oldScoreBetterKey = "oldScoreBetter_T";
    #endregion

    public LocalizationManager localizationManager;
    [HideInInspector]
    public HighscoreHandler highscorehandler;

    public int oldScore;
    public int newScore;

    private void Awake()
    {
        ///Highscore Handler is needed in every game scene,
        ///so it uses a singleton pattern to ensure it is always available and exists only once
        if (highscorehandler == null)
        {            
            highscorehandler = this;
        }
        else if (highscorehandler != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    public string GetHighscoreText()
    {
        oldScore = DataCollector.oldScore;
        newScore = DataCollector.currentScore;
        print("old: " + oldScore + " new: " + newScore);
        string bestScoreText = "";

        if (oldScore == newScore)
        {
            bestScoreText = string.Format(localizationManager.GetLocalizedText(scoresEqualKey), oldScore.ToString(), newScore.ToString());
        }
        else if (oldScore == -1000000)
        {
            bestScoreText = string.Format(localizationManager.GetLocalizedText(noPreviousScoreKey), newScore.ToString());
        }
        else if (oldScore < newScore)
        {
            bestScoreText = string.Format(localizationManager.GetLocalizedText(newScoreBetterKey), oldScore.ToString(), newScore.ToString());
        }
        else if (oldScore > newScore)
        {
            bestScoreText = string.Format(localizationManager.GetLocalizedText(oldScoreBetterKey), oldScore.ToString(), newScore.ToString());
        }
        return bestScoreText;
    }
}
