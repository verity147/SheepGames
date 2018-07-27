using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PW_SheepMovement : MonoBehaviour {

    public Vector2 force;

    private Rigidbody2D rBody;

	void Start () {
        rBody = GetComponent<Rigidbody2D>();
	}
	
    private void Push()
    {
        rBody.AddForce(force, ForceMode2D.Impulse);
    }

	void Update () {
        if (Input.GetButtonDown("Fire1"))
        {
            Push();
        }
	}
}
