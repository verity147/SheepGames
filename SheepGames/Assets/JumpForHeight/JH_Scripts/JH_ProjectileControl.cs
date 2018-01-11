using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JH_ProjectileControl : MonoBehaviour
{
    public float maxDragDist = 2f;

    internal bool isPressed = false;
    internal Vector2 flightVel;

    private bool readyToFly = false;
    private Rigidbody2D projectileRB;
    internal SpringJoint2D projSpringJoint;
    private Vector3 startPos;
    private JH_PlayerBody playerBody;
    private JH_GameController gameController;
    private Vector2 startJumpPos;
    private Vector2 mousePos;
    private Vector2 lastFrameVel;
    private float dragDist;

    private void Start()
    {
        playerBody = FindObjectOfType<JH_PlayerBody>();
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
        if (projSpringJoint.enabled == true && transform.position.x > startPos.x && readyToFly == true)
        {
            OnRunUp();
        }
        lastFrameVel = projectileRB.velocity;

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
            //count towards score
            projectileRB.isKinematic = false;
            readyToFly = true;
            playerBody.timeForRunUp = true;
        }
        else
        {
            FindObjectOfType<JH_GameController>().SpawnNewPlayer();
        }
          
    }

    private void OnRunUp()
    {
        readyToFly = false;
        projSpringJoint.enabled = false;
        flightVel = lastFrameVel;
        ///give playerBody the momentum acquired and disable self
        playerBody.timeForRunUp = false;
        playerBody.TakeOff();
        StartCoroutine(WaitAndDestroy());
    }

    private IEnumerator WaitAndDestroy()
    {
        yield return new WaitForEndOfFrame();
        Destroy(gameObject);
    }
}
