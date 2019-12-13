using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HR_DrinkCheck : MonoBehaviour
{
    private HR_Player player;
    private Collider2D myCollider;

    private void Awake()
    {
        player = GetComponentInParent<HR_Player>();
        myCollider = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (myCollider.IsTouchingLayers(LayerMask.GetMask("HR_Troughs")))
        {
            player.DrinkingAllowed = true;
        }        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.layer == 18)
        {
            player.DrinkingAllowed = false;
        }
    }
}
