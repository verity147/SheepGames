using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HR_Trough : MonoBehaviour
{
    private Animator animator;

    internal HR_Player player;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        animator.SetBool("drinking", player.drinking);
    }
}
