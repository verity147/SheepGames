using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;

public class PP_GameManager : MonoBehaviour {

    public Tilemap tilemap;

    private PP_PuzzlePartDisplay[] parts;
    private BoxCollider2D coll;
    private int correctParts = 0;
    private int numberOfTries = 0;
    private ContactFilter2D contactFilter;
    private LayerMask layerMask = 10;
    private int partCount = 0;
    private int scorePenalty = 10;
    private int scoreBonus = 100;

    private void Awake()
    {
        parts = transform.GetComponentsInChildren<PP_PuzzlePartDisplay>();
        coll = GetComponent<BoxCollider2D>();
    }

    private void Start()
    {
        foreach (PP_PuzzlePartDisplay part in parts)
        {
            FindPosInHoldingArea(part.transform);
        }
        contactFilter.SetLayerMask(layerMask);
        partCount = parts.Length;
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
        return(numberOfTries - partCount) * scorePenalty + partCount * scoreBonus;
    }

    private void GameFinished()
    {
        //do stuff, open menu
        print("You did it!");
        DataCollector.UpdateScore(CalculateScore());
    }

    internal void StartCheck(Transform puzzlePart)
    {
        ///check if the piece is inside the puzzle area...
        if (InPuzzleArea(puzzlePart.position))
        {
            ///...find the cell it was put on...
            Vector3 cellCenter = CellCenterFromClick(Input.mousePosition);
            Vector2 newPos = new Vector2(cellCenter.x, cellCenter.y);
            ///...put the piece exactly in the center...
            puzzlePart.position = newPos;
            ///...find out the correct position for the piece, adjusting for scaling of the tilemap...
            Vector3 correctPos = puzzlePart.GetComponent<PP_PuzzlePartDisplay>().part.correctPosition;
            float correctX = (correctPos).x * tilemap.transform.localScale.x;
            float correctY = (correctPos).y * tilemap.transform.localScale.y;
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
            ///if the piece lies incorrect, count as try
            else
            {
                numberOfTries++;
            }
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
        Vector3Int cell = tilemap.WorldToCell(worldPos);
        Vector3 cellCenter = tilemap.GetCellCenterWorld(cell);
        return cellCenter;
    }

    private bool InPuzzleArea(Vector3 pos)
    {
        if (tilemap.GetComponent<CompositeCollider2D>().bounds.Contains(pos))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    
    private void RestartLevel()
    {
        ///puts the pieces back in the Holding area and enables their collider
        foreach (PP_PuzzlePartDisplay part in parts)
        {
            part.GetComponent<Collider2D>().enabled = true;
            FindPosInHoldingArea(part.transform);
        }
        numberOfTries = 0;
    }
}
