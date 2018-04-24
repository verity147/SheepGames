using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PopulateHighscore : MonoBehaviour {

    public GameObject nameText;
    public GameObject valueText;
    public GameObject contentPrefab;
    public RectTransform viewportRect;
    public int highscoreLength;

    private GridLayoutGroup grid;
    private List<KeyValuePair<string, int>> result;

    private void Awake()
    {
        grid = GetComponentInChildren<GridLayoutGroup>();        
    }

    private void Start ()
    {
        //change to Total later 
        //NewGrid("JumpForHeight");
    }

    public void NewGrid()
    {
        Destroy(grid.gameObject);
        grid = Instantiate(contentPrefab, viewportRect.transform).GetComponent<GridLayoutGroup>();
        grid.cellSize = new Vector2(viewportRect.rect.width / 2, grid.cellSize.y);
        GetComponent<ScrollRect>().content = grid.gameObject.GetComponent<RectTransform>();
    }

    ///sorts score PER GAME
    public void NewGameScore(string game)
    {
        result = new List<KeyValuePair<string, int>>(DataCollector.GetGameTotals(DataCollector.gameLevels[game]));
        PopulateGrid(result);
    }

    ///sorts score PER LEVEL
    public void NewLevelScore(string level)
    {
        print("NewLevelScore");
        result = new List<KeyValuePair<string, int>>(DataCollector.SortScore(level));
        PopulateGrid(result);        
    }

    private void PopulateGrid(List<KeyValuePair<string, int>> scoreList)
    {
        GameObject entryName;
        GameObject entryValue;
        ///puts 1. etc in front of names
        int count = 1;
        foreach (KeyValuePair<string, int> entry in scoreList)
        {
            entryName = Instantiate(nameText, grid.transform);
            entryName.GetComponent<TMP_Text>().text = string.Concat(count.ToString(),". ", entry.Key);
            entryValue = Instantiate(valueText, grid.transform);
            entryValue.GetComponent<TMP_Text>().text = entry.Value.ToString();
            count++;
        }
    }

    public void PopulateTotal()
    {
        List<KeyValuePair<string, int>> result = DataCollector.GetScoreTotal();
        int count = 1;
        foreach (KeyValuePair<string, int> entry in result)
        {
            GameObject entryName = Instantiate(nameText, grid.transform);
            entryName.GetComponent<TMP_Text>().text = string.Concat(count.ToString(), ". ", entry.Key);
            GameObject entryValue = Instantiate(valueText, grid.transform);
            entryValue.GetComponent<TMP_Text>().text = entry.Value.ToString();
            count++;
        }
    }
}