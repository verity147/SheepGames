using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;

public class PP_PuzzleSetup : MonoBehaviour {

    [Range(1, 25)]
    public int puzzleWidth = 8;
    [Range(1, 25)]
    public int puzzleHeight = 8;
    
    public Tilemap puzzleArea;    
    public Vector2Int startingTile;
    public string pictureName;
    public GameObject part;

    internal List<GameObject> parts;

    private Sprite[] puzzlePicture;
    private PP_GameManager gameManager;

    private void Awake()
    {
        puzzlePicture = Resources.LoadAll<Sprite>(pictureName);
        gameManager = FindObjectOfType<PP_GameManager>();
        parts = new List<GameObject>();
    }

    private void Start()
    {
        gameManager.parts = GeneratePartList().ToArray();
        gameManager.FillHoldingArea();
    }

    private List<GameObject> GeneratePartList()
    {
        int counter = 0;
        foreach (Sprite picture in puzzlePicture)
        {
            GameObject newPart = Instantiate(part, gameManager.transform);
            ///assign the next sprite in the sprite sheet
            newPart.GetComponent<SpriteRenderer>().sprite = picture;
            ///fill the gameManager reference
            newPart.GetComponent<PP_PuzzlePartDisplay>().gameManager = gameManager;
            ///calculate the next position in the puzzle
            newPart.GetComponent<PP_PuzzlePartDisplay>().correctPosition = new Vector2Int((puzzleWidth + counter), (puzzleHeight + counter));
            ///add the part to the list
            parts.Add(newPart);
            counter++;
        }
        counter = 0;
        return parts;
    }
}
