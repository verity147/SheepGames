using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Allows PuzzleParts to be clicked and dragged and checks when they should be released
/// </summary>

public class PP_PuzzlePartDisplay : MonoBehaviour {


    internal PP_GameManager gameManager;
    internal Vector2Int correctPosition = new Vector2Int(0, 0);
    internal bool isPlaced = false;

    private bool isPressed = false;

    private void Awake()
    {
        //gameManager = GetComponentInParent<PP_GameManager>();
        //assign from setup

    }

    private void Start ()
    {
	}

    private void LateUpdate()
    {
        if (isPressed)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,Input.mousePosition.y));
            transform.position = new Vector3(mousePos.x, mousePos.y, transform.position.z);
        }
        if (Input.GetMouseButtonDown(1))
        {
            ///on right mouseclick, rotate the part 45 degrees
            Vector3 euler = transform.eulerAngles;
            euler.z = transform.rotation.z + 45;
            transform.eulerAngles = euler;
        }
    }

    private void OnMouseDown()
    {
        isPressed = true;
    }

    private void OnMouseUp()
    {
        isPressed = false;
        gameManager.CheckPartPosition(transform);
    }

}
