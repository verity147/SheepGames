using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PopulateHighscore : MonoBehaviour {

    public GameObject nameText;
    public GameObject valueText;
    public RectTransform parentRect;
    public int highscoreLength;

    private GridLayoutGroup grid;

    private void Awake()
    {
        grid = GetComponent<GridLayoutGroup>();
    }

    private void Start ()
    {
        grid.cellSize = new Vector2(parentRect.rect.width / 2, grid.cellSize.y);
        PopulateGrid("JH_GameLV_01");
	}

    public void PopulateGrid(string game)
    {
        foreach(KeyValuePair<string, int> entry in DataCollector.tempPlayerDict[game])
        {
            GameObject entryName = Instantiate(nameText, transform);
            entryName.GetComponent<TMP_Text>().text = entry.Key;
            GameObject entryValue = Instantiate(valueText, transform);
            valueText.GetComponent<TMP_Text>().text = entry.Value.ToString();
        }
    }

    private List<KeyValuePair<string, string>> BuildScoreList(string game)
    {
        return new List<KeyValuePair<string, string>>();
    }
}


//make list
//sort list
//instantiate prefab
//assign value to prefab
//show prefab
