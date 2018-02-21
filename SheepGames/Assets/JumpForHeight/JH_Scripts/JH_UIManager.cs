using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JH_UIManager : MonoBehaviour
{

    public Canvas canvas;
    public GameObject speechBubble;

    internal void ToggleExplanation(bool active)
    {
        speechBubble.SetActive(active);
    }
}
