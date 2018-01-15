using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JH_ScoreCalculator : MonoBehaviour {

    public Text scoreText;

    private int score;
    private int newScore;
    private int stonePointPenaltyStart = 50;
    private int stonePointPenalty = 50;
    private int winPointBonus = 1000;

    void Start () {
        //import current Score
        stonePointPenalty = stonePointPenaltyStart;
	}
    public void ResetPointPenalty()
    {
        stonePointPenalty = stonePointPenaltyStart;
        scoreText.text = "Score: 0";
    }

    private void OnTriggerEnter2D(Collider2D collider2D) 
    {
        if(collider2D.tag == "JH_Stone")
        {
            score -= stonePointPenalty;
            stonePointPenalty += stonePointPenaltyStart;
            scoreText.text = "Score: " + score.ToString();
        }
        else if(collider2D.tag != "JH_Stone" && collider2D.tag != "Player")
        {
            score += winPointBonus;
            scoreText.text = "Score: " + score.ToString();
        }

    }
}
