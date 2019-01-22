using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;

public class PP_GameManager : MonoBehaviour {

    public Tilemap puzzleArea;

    internal GameObject[] parts;
    private BoxCollider2D coll;
    private int correctParts = 0;
    private int numberOfTries = 0;
    private ContactFilter2D contactFilter;
    private LayerMask layerMask = 10;
    private int scorePenalty = 10;
    private int scoreBonus = 100;
    private readonly int[] rotations = { 0, 90, 180, 270 };

    private void Awake()
    {
        coll = GetComponent<BoxCollider2D>();
    }

    private void Start()
    {
        contactFilter.SetLayerMask(layerMask);
    }

    internal void FillHoldingArea()
    {
        foreach (GameObject part in parts)
        {
            FindPosInHoldingArea(part.transform);
            ///give the part a random rotation
            Vector3 euler = part.transform.eulerAngles;
            euler.z = rotations[Random.Range(0, rotations.Length)];
            part.transform.eulerAngles = euler;
        }
    }

    private void FindPosInHoldingArea(Transform part)
    {
        ///get random point in bounds
        Vector3 collExtents = coll.bounds.extents;
        Vector3 newPos = new Vector3(Random.Range(-collExtents.x, collExtents.x), Random.Range(-collExtents.y, collExtents.y), 0f);
        part.localPosition = newPos;
        if (part.GetComponent<Collider2D>().IsTouching(contactFilter))
        {
            //what did I want here???
        }
    }

    private int CalculateScore()
    {
        return(numberOfTries - parts.Length) * scorePenalty + parts.Length * scoreBonus;
    }

    private void GameFinished()
    {
        //do stuff, open menu
        print("You did it!");
        DataCollector.UpdateScore(CalculateScore());
    }

    internal void CheckPartPosition(Transform puzzlePart)
    {
        ///check if the piece is inside the puzzle area...
        if (puzzleArea.GetComponent<CompositeCollider2D>().bounds.Contains(puzzlePart.position))
        {
            if(puzzlePart.rotation.z == 0)
            {
                ///...find the cell it was put on...
                Vector3 cellCenter = CellCenterFromClick(Input.mousePosition);
                Vector2 newPos = new Vector2(cellCenter.x, cellCenter.y);
                ///...put the piece exactly in the center...
                puzzlePart.position = newPos;
                ///...find out the correct position for the piece, adjusting for scaling of the tilemap...
                Vector2 correctPos = puzzlePart.GetComponent<PP_PuzzlePartDisplay>().correctPosition;
                float correctX = (correctPos).x * puzzleArea.transform.localScale.x;
                float correctY = (correctPos).y * puzzleArea.transform.localScale.y;
                Vector2 correctPosLocal = new Vector2(correctX, correctY);
                ///check if the piece lies in its correct position
                if (newPos == correctPosLocal)
                {
                    print("correct");
                    ///stop the piece from being moved again
                    puzzlePart.GetComponent<Collider2D>().enabled = false;
                    correctParts++;
                    if (correctParts >= parts.Length)
                    {
                        GameFinished();
                    }
                }
                else
                {
                    FindPosInHoldingArea(puzzlePart);
                }
            }
            ///if the piece lies incorrect, put back in Holding Area
            else
            {
                FindPosInHoldingArea(puzzlePart);
            }
            ///count the try
            numberOfTries++;
        }
        ///if the piece is not within the puzzle area, put it back in the holding area
        else
        {
            FindPosInHoldingArea(puzzlePart);
        }
    }

    private Vector3 CellCenterFromClick(Vector3 mousePos)
    {
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
        Vector3Int cell = puzzleArea.WorldToCell(worldPos);
        Vector3 cellCenter = puzzleArea.GetCellCenterWorld(cell);
        return cellCenter;
    }
    
    private void RestartLevel()
    {
        ///puts the pieces back in the Holding area and enables their collider
        foreach (GameObject part in parts)
        {
            part.GetComponent<Collider2D>().enabled = true;
            FindPosInHoldingArea(part.transform);
        }
        numberOfTries = 0;
    }
}
