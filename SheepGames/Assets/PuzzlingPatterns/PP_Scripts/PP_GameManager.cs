using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;
using TMPro;

public class PP_GameManager : MonoBehaviour {

    public Tilemap puzzleArea;
    public Tilemap holdingArea;
    [Range(0, 64)]
    public int prePlaceParts = 0;
    public TMP_Text scoreText;
    public ParticleSystem winParticle;

    internal GameObject[] parts;
    //private BoxCollider2D holdingArea;
    private int correctParts = 0;
    private int numberOfTries = 0;
    private ContactFilter2D contactFilter;
    private LayerMask layerMask = 12;
    private int scorePenalty = 10;
    private int scoreBonus = 100;
    private readonly int[] rotations = { 0, 90, 180, 270 };
    private Bounds holdingAreaBounds;

    private void Awake()
    {
    }

    private void Start()
    {
        holdingAreaBounds = holdingArea.GetComponent<CompositeCollider2D>().bounds;
        print(holdingAreaBounds);
        contactFilter.SetLayerMask(layerMask);
    }

    internal void FillHoldingArea()
    {
        for (int i = 0; i < parts.Length; i++)
        {
            FindPosInHoldingArea(parts[i].transform);
            ///give the part a random rotation
            Vector3 euler = parts[i].transform.eulerAngles;
            euler.z = rotations[Random.Range(0, rotations.Length)];
            parts[i].transform.eulerAngles = euler;
        }    
    }

    private void FindPosInHoldingArea(Transform part)
    {
        ///get random point in bounds
        Vector3 collExtents = holdingAreaBounds.extents;
        ///z needs to be in front of background and gamemanager because clicks don't get properly registered otherwise
        Vector3 newPos = new Vector3(Random.Range(-collExtents.x, collExtents.x), 
                                     Random.Range(-collExtents.y, collExtents.y), -1f);
        newPos.x += holdingAreaBounds.center.x;
        newPos.y += holdingAreaBounds.center.y;
        part.position = newPos;
        if (part.GetComponent<Collider2D>().IsTouching(contactFilter))
        {
            FindPosInHoldingArea(part);
        }
    }

    private int CalculateScore()
    {
        return(numberOfTries - parts.Length - prePlaceParts) * scorePenalty + (parts.Length - prePlaceParts) * scoreBonus;
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
            ///have to check if the rotation is where it's supposed to be since it can't always be exactly zero
            if(puzzlePart.eulerAngles.z > -45 && puzzlePart.eulerAngles.z < 45)
            {
                ///...find the cell it was put on...
                Vector3 cellCenter = CellCenterFromClick(Input.mousePosition);
                print("Cellcenter: "+cellCenter);
                Vector2 newPos = new Vector2(cellCenter.x, cellCenter.y);
                ///...put the piece exactly in the center...
                puzzlePart.position = newPos;
                ///...find out the correct position for the piece, converting to localscale
                Vector2 correctPos = puzzlePart.GetComponent<PP_PuzzlePartDisplay>().correctPosition;
                float correctX = (correctPos).x * puzzleArea.transform.localScale.x;
                float correctY = (correctPos).y * puzzleArea.transform.localScale.y;
                Vector2 correctPosLocal = new Vector2(correctX, correctY);
                ///check if the piece lies in its correct position
                if (newPos == puzzlePart.GetComponent<PP_PuzzlePartDisplay>().correctPosition)
                {
                    print("correct");
                    ///stop the piece from being moved again
                    puzzlePart.GetComponent<Collider2D>().enabled = false;
                    correctParts++;
                    winParticle.transform.position = puzzlePart.transform.position;
                    winParticle.Play();
                    if (correctParts >= parts.Length)
                    {
                        GameFinished();
                    }
                }
                else
                {
                    print("wrong position");
                    ///count the try
                    numberOfTries++;
                    scoreText.text = "Versuche: " + numberOfTries;
                    FindPosInHoldingArea(puzzlePart);
                }
            }
            ///if the piece lies incorrect, put back in Holding Area
            else
            {
                print("rotated incorrectly");
                ///count the try
                numberOfTries++;
                scoreText.text = "Versuche: " + numberOfTries;
                FindPosInHoldingArea(puzzlePart);
            }
        }
        ///if the piece is not within the puzzle area, put it back in the holding area
        else
        {
            ///it is assumed the player wanted to put this part here
            if (holdingArea.GetComponent<CompositeCollider2D>().bounds.Contains(puzzlePart.position))
                return;

            print("not in puzzleArea or holding area");
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
