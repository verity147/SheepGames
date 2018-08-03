using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PW_InputManager : MonoBehaviour {

    private BoxCollider2D boxCollider;
    private ContactFilter2D contactFilter;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        contactFilter = new ContactFilter2D();
        contactFilter.layerMask = 9;
    }

    private void Update()
    {
        if (Input.GetButtonDown("Left"))
        {
            //0
            CheckForDirection();
        }
        if (Input.GetButtonDown("Right"))
        {
            //1
        }
        if (Input.GetButtonDown("Up"))
        {
            //2
        }
        if (Input.GetButtonDown("Down"))
        {
            //3
        }

    }

    private void CheckForDirection()
    {
        //OverlapCollider is not working!!!
        Collider2D[] touchingColliders = new Collider2D[20];
        boxCollider.OverlapCollider(contactFilter.NoFilter(), touchingColliders);

        foreach(Collider2D coll in touchingColliders)
        {
            print(coll.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        collision.gameObject.SetActive(false);
        //count as missed hit
    }
}
