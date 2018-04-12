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

    private void Awake()
    {
        grid = GetComponentInChildren<GridLayoutGroup>();
    }

    private void Start ()
    {
        //change to Total later 
        NewGrid("JumpForHeight");
    }

    public void NewGrid(string game)
    {
        
        Destroy(grid.gameObject);
        grid = Instantiate(contentPrefab, viewportRect.transform).GetComponent<GridLayoutGroup>();
        grid.cellSize = new Vector2(viewportRect.rect.width / 2, grid.cellSize.y);
        PopulateGrid(game);
    }

    private void PopulateGrid(string game)
    {
        List<KeyValuePair<string, int>> result = DataCollector.GetGameTotals(DataCollector.gameLevels[game]);
        GameObject entryName;
        GameObject entryValue;
        ///puts 1. etc in front of names
        int count = 1;
        foreach (KeyValuePair<string, int> entry in result)
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
        Destroy(grid.gameObject);
        grid = Instantiate(contentPrefab, viewportRect.transform).GetComponent<GridLayoutGroup>();
        grid.cellSize = new Vector2(viewportRect.rect.width / 2, grid.cellSize.y);
        List<KeyValuePair<string, int>> result = DataCollector.GetScoreTotal();
        foreach (KeyValuePair<string, int> entry in result)
        {
            GameObject entryName = Instantiate(nameText, grid.transform);
            entryName.GetComponent<TMP_Text>().text = entry.Key;
            GameObject entryValue = Instantiate(valueText, grid.transform);
            valueText.GetComponent<TMP_Text>().text = entry.Value.ToString();
        }
    }
}