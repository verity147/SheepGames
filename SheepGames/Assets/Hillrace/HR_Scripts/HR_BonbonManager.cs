using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HR_BonbonManager : MonoBehaviour
{
    public HR_Gamemanager gamemanager;

    internal int childCount;

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

        childCount = bonbons.Count;
    }

    internal void ResetBonbons()
    {
        foreach(HR_Bonbon bonbon in bonbons)
        {
            bonbon.gameObject.SetActive(true);
        }
    }
}
