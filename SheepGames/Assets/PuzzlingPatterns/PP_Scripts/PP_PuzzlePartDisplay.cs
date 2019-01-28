using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Allows PuzzleParts to be clicked and dragged and checks when they should be released
/// </summary>

public class PP_PuzzlePartDisplay : MonoBehaviour {


    internal PP_GameManager gameManager;
    internal Vector2 correctPosition = new Vector2(0, 0);
    internal bool isPlaced = false;

    private bool isPressed = false;

    private void LateUpdate()
    {
        if (isPressed)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,Input.mousePosition.y));
            transform.position = new Vector3(mousePos.x, mousePos.y, transform.position.z);
            if (Input.GetMouseButtonDown(1))
            {
                ///on right mouseclick, rotate the part 90 degrees
                Vector3 euler = transform.eulerAngles;
                euler.z = transform.eulerAngles.z + 90;
                transform.eulerAngles = euler;
            }
        }
    }

    internal void PrePlaced()
    {
        GetComponent<Collider2D>().enabled = false;
        print("Chosen for pre-place: " + GetComponent<SpriteRenderer>().sprite);
    }

    //private void OnMouseDown()
    //{
    //    isPressed = true;
    //}

    //private void OnMouseUp()
    //{
    //    isPressed = false;
    //    gameManager.CheckPartPosition(transform);
    //}

    private void OnMouseUp()
    {
        isPressed = !isPressed;
        if (isPressed == false)
        {
            gameManager.CheckPartPosition(transform);
        }
    }
}
