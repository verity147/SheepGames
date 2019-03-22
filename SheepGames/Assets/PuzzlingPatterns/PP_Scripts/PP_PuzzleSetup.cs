using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.Tilemaps;
using UnityEngine;

public class PP_PuzzleSetup : MonoBehaviour {

    [Range(1, 25)]
    public int puzzleWidthInCells = 8;
    [Range(1, 25)]
    public int puzzleHeightInCells = 8;

    
    [Tooltip("Needs to be the top left tile of the puzzle area")]
    public Vector2 startingTile;
    [Tooltip("Requires Sprite in multiple sprite mode, rows and columns matching the puzzlesize!")]
    public string pictureName;
    public GameObject part;

    private Sprite[] puzzlePicture;
    private Tilemap puzzleArea;    
    private PP_GameManager gameManager;
    private Vector2 puzzleSize;
    private List<GameObject> parts;

    private void Awake()
    {
        puzzlePicture = Resources.LoadAll<Sprite>(pictureName);
        gameManager = FindObjectOfType<PP_GameManager>();
        puzzleArea = gameManager.puzzleArea;
        parts = new List<GameObject>();
    }

    private void Start()
    {
        puzzleSize.x = puzzleArea.cellSize.x * puzzleWidthInCells;
        puzzleSize.y = puzzleArea.cellSize.y * puzzleHeightInCells;

        //if this is not the tutorial/startscreen
        if(SceneHandler.GetSceneName() != "PP_GameLV_00")
            DistributeParts();
    }

    public void DistributeParts()
    {
        gameManager.parts = GeneratePartList().ToArray();
        gameManager.FillHoldingArea();
        if (gameManager.prePlaceParts > 0)
            PrePlaceParts();
    }

    private List<GameObject> GeneratePartList()
    {
        Vector2 nextPos = startingTile;
        foreach (Sprite picture in puzzlePicture)
        {
            GameObject newPart = Instantiate(part, gameManager.transform);
            ///assign the next sprite in the sprite sheet
            newPart.GetComponent<SpriteRenderer>().sprite = picture;
            ///fill the gameManager reference
            newPart.GetComponent<PP_PuzzlePartDisplay>().gameManager = gameManager;
            ///calculate the next position in the puzzle
            newPart.GetComponent<PP_PuzzlePartDisplay>().correctPosition = nextPos;
            nextPos = FindCorrectPos(nextPos);
            ///add the part to the list
            parts.Add(newPart);
        }
        return parts;
    }

    private Vector2 FindCorrectPos(Vector2 nextPos)
    {
        if(nextPos.x < startingTile.x + puzzleSize.x - puzzleArea.cellSize.x)
        {
            nextPos.x += puzzleArea.cellSize.x;
        }
        else
        {
            nextPos.y -= puzzleArea.cellSize.y;
            nextPos.x = startingTile.x;
        }
        return nextPos;
    }

    private void PrePlaceParts()
    {
        List<int> usedNumbers = new List<int>();
        int newRandomNumber;

        while (usedNumbers.Count < gameManager.prePlaceParts)
        {
            newRandomNumber = Random.Range(0, 64);

            if (!usedNumbers.Contains(newRandomNumber))
            {
                usedNumbers.Add(newRandomNumber);
                PP_PuzzlePartDisplay puzzlePart = parts[newRandomNumber].GetComponent<PP_PuzzlePartDisplay>();
                puzzlePart.transform.eulerAngles = Vector3.zero;
                puzzlePart.transform.position = puzzlePart.correctPosition;
                puzzlePart.PrePlaced();
            }
        }
    }
}

            
