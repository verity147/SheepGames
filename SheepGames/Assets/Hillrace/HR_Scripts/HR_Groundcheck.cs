using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HR_Groundcheck : MonoBehaviour
{
    private HR_Player player;

    private void Awake()
    {
        player = GetComponentInParent<HR_Player>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 15)
        {
            player.isGrounded = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 15)
        {
            player.isGrounded = false;
        }
    }
}
