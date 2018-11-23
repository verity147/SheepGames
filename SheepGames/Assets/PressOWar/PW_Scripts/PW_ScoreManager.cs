using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public enum ScoreMalus
    {
        WrongDirPressed = 0,
        DirMissed = 1,
        PressedNoDir = 2
    }

public class PW_ScoreManager : MonoBehaviour {

    public float maxScoreMult = 2f;
    public int scoreBonus = 10;
    public int wrongDirPressed = 10;
    public int dirMissed = 5;
    public int pressedNoDir = 5;

    public ScoreMalus malus;

    internal float currentScore = 0;

    private PW_UIHandler uiHandler;

    private void Awake()
    {
        uiHandler = FindObjectOfType<PW_UIHandler>();
    }

    public void AddScore(float scoreMult)
    {
        currentScore += scoreBonus * scoreMult;
        UpdateScore();
    }

    public void SubstractScore(ScoreMalus malus)
    {
        switch (malus)
        {
            case ScoreMalus.WrongDirPressed:
                currentScore -= wrongDirPressed;
                break;
            case ScoreMalus.DirMissed:
                currentScore -= dirMissed;
                break;
            case ScoreMalus.PressedNoDir:
                currentScore -= pressedNoDir;
                break;
            default:
                break;
        }
        UpdateScore();
    }

    public void ResetScore()
    {
        currentScore = 0;
        UpdateScore();
    }

    internal void UpdateScore()
    {
        int scoreInt = Mathf.RoundToInt(currentScore);
        uiHandler.score.text = scoreInt.ToString();
    }

    internal void UpdateScore(int addScore)
    {
        int scoreInt = Mathf.RoundToInt(currentScore) + addScore;
        uiHandler.score.text = scoreInt.ToString();
        DataCollector.currentScore = Mathf.RoundToInt(scoreInt);
    }
}
