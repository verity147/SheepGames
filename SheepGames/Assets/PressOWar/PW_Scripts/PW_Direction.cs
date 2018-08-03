using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PW_Direction : MonoBehaviour {

    public float moveSpeed = 5f;

    private void Update()
    {
        transform.position += Vector3.down * Time.deltaTime * moveSpeed;
    }
}
