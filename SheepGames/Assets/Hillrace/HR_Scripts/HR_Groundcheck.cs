using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HR_Groundcheck : MonoBehaviour
{
    private HR_Player player;
    private Collider2D myCollider;

    private void Awake()
    {
        player = GetComponentInParent<HR_Player>();
        myCollider = GetComponent<Collider2D>();
    }

    private void Update()
    {
        ///add more layers here if the player is supposed to be able to jump from anywhere else
        player.isGrounded = myCollider.IsTouchingLayers(LayerMask.GetMask("Ground"))|| 
                            myCollider.IsTouchingLayers(LayerMask.GetMask("Water"));
        player.isSwimming = myCollider.IsTouchingLayers(LayerMask.GetMask("Water"));
    }
}
