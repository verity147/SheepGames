using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public enum Direction
    {
        Left = 0,
        Right = 1,
        Up = 2,
        Down = 3
    }

public class PW_Direction : MonoBehaviour {

    public float moveSpeed = 5f;

    public Direction direction;

    private void Update()
    {
        transform.position += Vector3.down * Time.deltaTime * moveSpeed;        
    }
}
