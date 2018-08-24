using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PW_ScoreManager : MonoBehaviour {

    public float maxScoreMult = 2f;
    public int scoreBonus = 10;
    public int wrongDirPressed = 10;
    public int dirMissed = 5;
    public int pressedNoDir = 5;

    private float currentScore = 0;

    public void AddScore(float scoreMult)
    {
        currentScore += scoreBonus * scoreMult;
    }

    public void SubstractScore()
    {

    }

    public void ResetScore()
    {
        currentScore = 0;
    }
}
