using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PW_InputManager : MonoBehaviour {

    private BoxCollider2D boxCollider;
    private ContactFilter2D contactFilter;
    private PW_ScoreManager scoreManager;

    private void Awake()
    {
        scoreManager = FindObjectOfType<PW_ScoreManager>();
        boxCollider = GetComponent<BoxCollider2D>();
        contactFilter = new ContactFilter2D
        {
            layerMask = 9
        };
    }

    private void Update()
    {
        if (Input.GetButtonDown("Left"))
        {
            CheckForDirection(Direction.Left);
        }
        if (Input.GetButtonDown("Right"))
        {
            CheckForDirection(Direction.Right);
        }
        if (Input.GetButtonDown("Up"))
        {
            CheckForDirection(Direction.Up);
        }
        if (Input.GetButtonDown("Down"))
        {
            CheckForDirection(Direction.Down);
        }

    }

    private void CheckForDirection(Direction inputDir)
    {
        Collider2D[] touchingColliders = new Collider2D[20];
        int colliderAmount = boxCollider.OverlapCollider(contactFilter.NoFilter(), touchingColliders);

        if (colliderAmount > 0 && colliderAmount < 2)
        {
            foreach(Collider2D coll in touchingColliders)
            {
                if (coll)
                {

                    print(coll.gameObject);
                    if(inputDir == coll.GetComponent<PW_Direction>().direction)
                    {
                        print("correct");
                        PrecisionCheck(coll.gameObject);
                    }
                    else
                    {
                        print("wrong");
                    }
                    //do some effect instead of just disabling
                    coll.gameObject.SetActive(false);
                }
            }
        }
        else if(colliderAmount>1)
        {
            ///at least 0.45 seems a sensible time for min. Direction spawn wait time
            Debug.LogError("DebugSpawner's min. spawn wait time is too low, more than one direction touchend the Input Manager!");
        }
        else
        {
            //did not hit any direction
        }
    }

    private void PrecisionCheck(GameObject direction)
    {
        float deltaY = Mathf.Abs(direction.transform.position.y - transform.position.y);
        print(deltaY);
        ///the multiplier should never diminish the score, so it gets clamped with a min of 1
        float scoreMult = 1f;
        if (deltaY > 0.05f)
        {
            scoreMult = Mathf.Clamp((scoreManager.maxScoreMult - deltaY), 1f, scoreManager.maxScoreMult);
        }
        else
        {
            ///if the player was particularly precise, he gets the maximum multiplier possible
            scoreMult = scoreManager.maxScoreMult;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        collision.gameObject.SetActive(false);
        //count as missed hit
    }
}
