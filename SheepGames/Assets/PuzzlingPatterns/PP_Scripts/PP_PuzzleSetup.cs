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

    
    public Tilemap puzzleArea;    
    [Tooltip("Needs to be the top left tile of the puzzle area")]
    public Vector2 startingTile;
    [Tooltip("Requires Sprite in multiple sprite mode, rows and columns matching the puzzlesize!")]
    public string pictureName;
    public GameObject part;

    private Sprite[] puzzlePicture;
    private PP_GameManager gameManager;
    private Vector2 puzzleSize;
    private List<GameObject> parts;

    private void Awake()
    {
        puzzlePicture = Resources.LoadAll<Sprite>(pictureName);
        //FileStream stream = new FileStream(Path.Combine(Application.streamingAssetsPath, "PP_PuzzlePicture.png"), FileMode.Open);
        //Sprite sprite = ImageConversion.LoadImage(stream);
        //puzzlePicture = (Sprite)Path.Combine(Application.streamingAssetsPath, "PP_PuzzlePicture.png");
        gameManager = FindObjectOfType<PP_GameManager>();
        parts = new List<GameObject>();
    }

    private void Start()
    {
        puzzleSize.x = puzzleArea.cellSize.x * puzzleWidthInCells;
        puzzleSize.y = puzzleArea.cellSize.y * puzzleHeightInCells;
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
        for (int i = 0; i <= gameManager.prePlaceParts; i++)
        {
            int j = Random.Range(0, 64);

            PP_PuzzlePartDisplay puzzlePart = parts[j].GetComponent<PP_PuzzlePartDisplay>();
            puzzlePart.transform.eulerAngles = Vector3.zero;
            puzzlePart.transform.position = puzzlePart.correctPosition;
            puzzlePart.PrePlaced();
        }
    }
}
