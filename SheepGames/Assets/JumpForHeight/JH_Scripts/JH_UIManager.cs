using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JH_UIManager : MonoBehaviour
{

    public Canvas canvas;
    public GameObject speechBubble;

    private int tries = 0;
    private int maxTries = 2;

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
