using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JH_UIManager : MonoBehaviour
{

    public Canvas canvas;
    public GameObject speechBubble;

    private JH_ScoreCalculator scoreCalculator;
    private int tries = 0;
    private int maxTries = 2;

    private void Awake()
    {
        scoreCalculator = FindObjectOfType<JH_ScoreCalculator>();
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
            //hide retry button
            DataCollector.CalculateTotalScore();
        }
    }
}
