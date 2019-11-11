using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HR_Spring : MonoBehaviour
{

    public HR_Player player;
    public float force = 10f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        print(collision.gameObject);
        if (collision.gameObject.tag=="Player")
        {
            print("player");

            //use ApplyJumpForce() in Player script, but find a way to use it until jumpHeight has been reached
            //edit jumpHeight, AddForce() doesn't work with this rigidbody setup
            //maybe use jumpHeight + jumpBoost for height value (build boosted vs unboosted jump logic?)
            GetComponentInChildren<Animator>().SetTrigger("spring");
        }
    }
}
