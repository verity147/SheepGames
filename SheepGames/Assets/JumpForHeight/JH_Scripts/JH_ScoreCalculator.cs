using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JH_ScoreCalculator : MonoBehaviour {
    
    internal int score = 0;
    private readonly int obstacleSheepPenalty = 250;
    internal int stonePointPenalty = 20;
    internal readonly int winPointBonus = 200;
    private readonly int flawlessBonus = 50;
    private List<GameObject> countedObjects;
    private bool flawless = true;

    private void Start()
    {
        countedObjects = new List<GameObject>();
    }

    public void ResetScore()
    {
        score = 0;
        flawless = true;
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
                flawless = false;
            }
            else if(obj.tag == "JH_Heather")
            {
                score += winPointBonus;
            }
            else if (obj.tag == "Obstacle")
            {
                score -= obstacleSheepPenalty;
                flawless = false;
            }
        }
        //paint all counted stones red for clarification/testing
        //if (obj.GetComponent<SpriteRenderer>())
        //{
        //    obj.GetComponent<SpriteRenderer>().color = Color.red;
        //}

    }

    internal int CalculateEndScore()
    {
        if (score == 0)
            flawless = false;
        return flawless == true ? score + flawlessBonus : score;
    }
}