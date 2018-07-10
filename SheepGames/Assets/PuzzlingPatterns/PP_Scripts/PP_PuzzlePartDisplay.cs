using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PP_PuzzlePartDisplay : MonoBehaviour {

    public PP_PuzzlePart part;

    public SpriteRenderer partPicture;

    private bool isPressed = false;
    private PP_GameManager gameManager;

    private void Awake()
    {
        gameManager = GetComponentInParent<PP_GameManager>();
    }

    private void Start () {
        partPicture.sprite = part.picture;
	}

    private void LateUpdate()
    {
        if (isPressed)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,Input.mousePosition.y));
            transform.position = new Vector3(mousePos.x, mousePos.y, transform.position.z);
        }
    }

    private void OnMouseDown()
    {
        isPressed = true;
    }

    private void OnMouseUp()
    {
        isPressed = false;
        gameManager.StartCheck(transform);
    }

}
