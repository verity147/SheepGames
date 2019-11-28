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

    private void FixedUpdate()
    {
        ///add more layers here if the player is supposed to be able to jump from anywhere else
        player.IsGrounded = myCollider.IsTouchingLayers(LayerMask.GetMask("Ground"))|| 
                            myCollider.IsTouchingLayers(LayerMask.GetMask("Water")) ||
                            myCollider.IsTouchingLayers(LayerMask.GetMask("HR_Mud"));
        player.isSwimming = myCollider.IsTouchingLayers(LayerMask.GetMask("Water"));
        player.inMud = myCollider.IsTouchingLayers(LayerMask.GetMask("HR_Mud"));
    }
}
