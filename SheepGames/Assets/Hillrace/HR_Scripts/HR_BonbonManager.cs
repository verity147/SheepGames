using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HR_BonbonManager : MonoBehaviour
{
    public HR_Gamemanager gamemanager;

    private List<HR_Bonbon> bonbons;

    private void Awake()
    {
        bonbons = new List<HR_Bonbon>(GetComponentsInChildren<HR_Bonbon>());
    }

    private void Start()
    {
        foreach (HR_Bonbon bonbon in bonbons)
        {
            bonbon.gameManager = gamemanager;
        }   
    }
}
