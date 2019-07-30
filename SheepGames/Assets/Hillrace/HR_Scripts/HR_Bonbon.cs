using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HR_Bonbon : MonoBehaviour
{
    public HR_Gamemanager gameManager;

    private void Awake()
    {
        gameManager = FindObjectOfType<HR_Gamemanager>();
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {

        if (coll.gameObject.tag == "Player")
        {
            gameManager.CollectPoint();
            gameObject.SetActive(false);
        }
    }
}
