using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HR_Bonbon : MonoBehaviour
{
    internal HR_Gamemanager gameManager;


    private void OnTriggerEnter2D(Collider2D coll)
    {

        if (coll.gameObject.tag == "Player")
        {
            gameManager.CollectPoint();
            gameObject.SetActive(false);
        }
    }
}
