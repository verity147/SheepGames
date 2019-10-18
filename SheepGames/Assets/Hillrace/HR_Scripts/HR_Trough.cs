using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HR_Trough : MonoBehaviour
{
    private Animator animator;
    internal BoxCollider2D boxCollider;

    internal HR_Player player;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        if (player.drinking && boxCollider.IsTouchingLayers(LayerMask.GetMask("Player")))
        {
            animator.SetBool("drinking", true);
            if (Input.GetButtonUp("Down"))
            {
                animator.SetBool("drinking", false);

            }
        }       
    }


}
