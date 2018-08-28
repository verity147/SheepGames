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

    private float currentScore = 0;

    public void AddScore(float scoreMult)
    {
        currentScore += scoreBonus * scoreMult;
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
    }

    public void ResetScore()
    {
        currentScore = 0;
    }
}
