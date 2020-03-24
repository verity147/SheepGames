using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JH_ScoreCalculator : MonoBehaviour {
    
    internal int score = 0;
    private readonly int stonePointPenaltyStart = 30;
    private readonly int obstacleSheepPenalty = 250;
    internal int stonePointPenalty = 30;
    internal readonly int winPointBonus = 300;
    private List<GameObject> countedObjects;

    void Start () {
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
            else if(obj.tag == "JH_Heather")
            {
                score += winPointBonus;
            }
            else if (obj.tag == "Obstacle")
            {
                score -= obstacleSheepPenalty;
            }
        }
        //paint all counted stones red for clarification/testing
        //if (obj.GetComponent<SpriteRenderer>())
        //{
        //    obj.GetComponent<SpriteRenderer>().color = Color.red;
        //}

    }


}