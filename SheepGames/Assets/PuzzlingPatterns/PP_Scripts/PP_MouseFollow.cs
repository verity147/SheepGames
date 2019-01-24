using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PP_MouseFollow : MonoBehaviour {

    private Vector3 newPos;

	private void Update ()
    {   
        ///for unknown reasons the z positions drifts of towards infinity,
        ///this was the easiest way to stop it
        newPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        newPos.z = -10f;
        transform.position = newPos;
	}
}
