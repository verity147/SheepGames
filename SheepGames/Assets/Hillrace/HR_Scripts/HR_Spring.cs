using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HR_Spring : MonoBehaviour
{
    internal HR_Player player;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject == player.gameObject)
        {
            GetComponentInChildren<Animator>().SetTrigger("spring");
            player.PrepareJump(HR_Player.Jumpstate.spring);
        }
    }
}
