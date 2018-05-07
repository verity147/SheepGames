using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JH_ScoreCalculator : MonoBehaviour {


    internal int score;

    private int newScore;
    private int stonePointPenaltyStart = 100;
    internal int stonePointPenalty = 100;
    internal int winPointBonus = 1000;
    private List<GameObject> countedObjects;

    void Start () {
        //import current Score
        //score needs to be reset on retry and kept for the next level
        stonePointPenalty = stonePointPenaltyStart;
        countedObjects = new List<GameObject>();
    }
    public void ResetScore()
    {
        score = 0;
        stonePointPenalty = stonePointPenaltyStart;
        countedObjects.Clear();
        countedObjects.TrimExcess();
    }

    private void OnTriggerEnter2D(Collider2D obj) 
    {
        if (!countedObjects.Contains(obj.gameObject))
        {
            countedObjects.Add(obj.gameObject);

            if(obj.tag == "JH_Stone")
            {
                score -= stonePointPenalty;
            }
            else if(obj.tag != "JH_Stone" && obj.tag != "Player")
            {
                score += winPointBonus;
            }
        }
        if (obj.GetComponent<SpriteRenderer>())
        {
            obj.GetComponent<SpriteRenderer>().color = Color.red;
        }

    }


}