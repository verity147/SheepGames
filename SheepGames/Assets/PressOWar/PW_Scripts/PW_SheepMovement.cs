using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PW_SheepMovement : MonoBehaviour {

    public Vector2 force;

    private Rigidbody2D rBody;

    private void Awake()
    {
        rBody = GetComponent<Rigidbody2D>();        
    }

    private void Start () {
	}
	
    private void Push()
    {
        rBody.AddForce(force, ForceMode2D.Impulse);
    }

	private void Update () {
        if (Input.GetButtonDown("Fire1"))
        {
            Push();
        }
	}
}
