using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Group : MonoBehaviour {

    public GameObject prefab;
    public int numberOfTexts;
    public GameObject content;
    public RectTransform parent;
    private GridLayoutGroup grid;

    private void Awake()
    {
        grid = GetComponentInChildren<GridLayoutGroup>();
        parent = GetComponentInChildren<Mask>().rectTransform;
    }

    private void Start()
    {
        NewGrid();
    }

    public void NewGrid()
    {
        Destroy(grid.gameObject);
        grid = Instantiate(content, parent.transform).GetComponent<GridLayoutGroup>();
        //grid.cellSize = new Vector2(parent.rect.width / 2, grid.cellSize.y);
        Populate();
    }

    private void Populate()
    {
        GameObject newObj;

        for (int i = 0; i < numberOfTexts; i++)
        {
            newObj = Instantiate(prefab, grid.transform);
            newObj.GetComponent<Text>().text = (i).ToString();
        }
    }
}
