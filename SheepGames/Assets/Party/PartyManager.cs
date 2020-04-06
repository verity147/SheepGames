using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyManager : MonoBehaviour
{
    public GameObject menu;

    public void Awake()
    {
        PopulateHighscore populateHighscore = menu.GetComponentInChildren<PopulateHighscore>();
        populateHighscore.NewGrid();
        populateHighscore.PopulateTotal();
    }
}
