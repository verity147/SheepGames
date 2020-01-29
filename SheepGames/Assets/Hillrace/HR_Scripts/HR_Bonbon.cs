using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HR_Bonbon : MonoBehaviour
{
    internal HR_Gamemanager gameManager;


    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.GetType()==typeof(BoxCollider2D) && coll.gameObject.tag == "Player") ///only trigger on one of the player's colliders
        {
            gameManager.CollectPoint();
            gameObject.SetActive(false);
        }
    }   
}
