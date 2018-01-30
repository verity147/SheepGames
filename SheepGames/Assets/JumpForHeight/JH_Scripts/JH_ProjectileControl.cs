using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JH_ProjectileControl : MonoBehaviour
{
    public float maxDragDist = 2f;

    internal bool isPressed = false;


    private Rigidbody2D projectileRB;
    internal SpringJoint2D projSpringJoint;
    private Vector3 startPos;
    private JH_PlayerBody player;
    private JH_GameController gameController;
    private Vector2 startJumpPos;
    private Vector2 mousePos;
    private Vector2 lastFrameVel;
    private float dragDist;

    private void Start()
    {
        player = FindObjectOfType<JH_PlayerBody>();
        gameController = FindObjectOfType<JH_GameController>();
        projectileRB = GetComponent<Rigidbody2D>();
        projSpringJoint = GetComponent<SpringJoint2D>();
        projectileRB.isKinematic = true;
        startPos = transform.position;
        startJumpPos = transform.position;
        projSpringJoint.connectedBody = gameController.GetComponent<Rigidbody2D>();
        lastFrameVel = Vector2.zero;
    }

    private void Update()
    {
        ///follow mouse when pressed...
        if (isPressed)
        {
            ///how far the mouse is from the startJump
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            dragDist = Vector3.Distance(mousePos, startJumpPos);
            ///...but not farther than maxDragDist
            if (dragDist > maxDragDist)
            {
                projectileRB.position = startJumpPos + (mousePos - startJumpPos).normalized * maxDragDist;
            }
            else
            {
                projectileRB.position = mousePos;
            }
        }
        
        lastFrameVel = projectileRB.velocity;

        if(transform.position.x > gameController.transform.position.x)
        {
            projSpringJoint.enabled = false;
            player.flightVel = lastFrameVel;
            player.MovePlayerToStart();
            StartCoroutine(WaitAndDestroy());
        }
    }

    private void OnMouseDown()
    {
        isPressed = true;
    }

    private void OnMouseUp()
    {
        isPressed = false;
        ///if player was only moved a negligible distance, respawn without doing a jump and without adding to score
        if (dragDist > 0.5f)
        {
            projectileRB.isKinematic = false;
            OnRunUp();
        }
        else
        {
            FindObjectOfType<JH_GameController>().SpawnNewPlayer();
        }
          
    }

    private void OnRunUp()
    {
        ///give playerBody the momentum acquired and disable self
    }

    private IEnumerator WaitAndDestroy()
    {
        yield return new WaitForEndOfFrame();
        Destroy(gameObject);
    }
}
