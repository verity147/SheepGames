using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopulateHighscore : MonoBehaviour {

    public GameObject nameText;
    public GameObject valueText;
    public int highscoreLength;

    private GridLayoutGroup grid;

    private void Awake()
    {
        grid = GetComponent<GridLayoutGroup>();
    }

    void Start () {
        PopulateGrid();
	}

    private void PopulateGrid()
    {

        for (int i = 0; i < highscoreLength; i++)
        {
            Instantiate(nameText, transform);
        }
    }
}
