﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JH_ScoreCalculator : MonoBehaviour {

    public Text scoreText;

    private int score;
    private int newScore;
    private int stonePointPenaltyStart = 100;
    private int stonePointPenalty = 100;
    private int winPointBonus = 1000;

    void Start () {
        //import current Score
        //score needs to be reset on retry and kept for the next level
        stonePointPenalty = stonePointPenaltyStart;
	}
    public void ResetPointPenalty()
    {
        score = 0;
        stonePointPenalty = stonePointPenaltyStart;
        scoreText.text = "Score: " + score.ToString();
    }

    private void OnTriggerEnter2D(Collider2D collider2D) 
    {
        print(collider2D.name);
        if(collider2D.tag == "JH_Stone")
        {
            score -= stonePointPenalty;
            //stonePointPenalty += stonePointPenaltyStart;
            scoreText.text = "Score: " + score.ToString();
        }
        else if(collider2D.tag != "JH_Stone" && collider2D.tag != "Player")
        {
            score += winPointBonus;
            scoreText.text = "Score: " + score.ToString();
        }
        if (collider2D.GetComponent<SpriteRenderer>())
        {
            collider2D.GetComponent<SpriteRenderer>().color = Color.red;
        }

    }
}
