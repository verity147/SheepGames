using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PP_MouseFollow : MonoBehaviour {


	private void Update () {
        transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
	}
}
