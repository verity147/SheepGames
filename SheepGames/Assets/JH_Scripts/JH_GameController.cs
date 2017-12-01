using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class JH_GameController : MonoBehaviour {

    public Transform projPrefab;
    public Transform playerPrefab;
    public Transform wallPrefab;
    public Transform[] toBeMovedInParallax;
    public Camera mCam; /// main camera
    public CinemachineVirtualCameraBase staticVCam;
    public CinemachineVirtualCameraBase movingVCam;
    public float smoothFactor = 1f;


    private Vector3 wallPos;
    private Vector3 prevCamPos;
    private Transform tempProj;
    private Transform tempWall;
    private Transform tempPlayer;
    private List<Vector3> backgroundStartPos;

    //use this script to retain trail from previous tries

    private void Start()
    {
        wallPos = wallPrefab.position;
        prevCamPos = movingVCam.transform.position;

        backgroundStartPos = new List<Vector3>();

        foreach(Transform obj in toBeMovedInParallax)
            {
                backgroundStartPos.Add(obj.position);
            }
        SpawnNewPlayer();
        staticVCam.MoveToTopOfPrioritySubqueue();
    }


    private void LateUpdate()
    {
        float camDist = 0f;
        if(mCam.GetComponent<CinemachineBrain>().ActiveVirtualCamera.Name == movingVCam.Name)
        {
            camDist = Mathf.Abs(movingVCam.transform.position.x - prevCamPos.x);
            foreach(Transform bgElement in toBeMovedInParallax)
            {
                float nextX = bgElement.position.x - camDist; //greater z equals smaller nextX
                bgElement.position = new Vector3(nextX, bgElement.position.y, bgElement.position.z);
            }
        }
            prevCamPos = movingVCam.transform.position;
    }

    internal void SwitchCamera()
    {
        if (mCam.GetComponent<CinemachineBrain>().ActiveVirtualCamera.Name == staticVCam.Name)
        {
            movingVCam.MoveToTopOfPrioritySubqueue();
        }
        else
            staticVCam.MoveToTopOfPrioritySubqueue();
    }


    public void SpawnNewPlayer()
    {
        ///reset backgrounds
       for(int i = 0; i < toBeMovedInParallax.Length; i++)
            {
                toBeMovedInParallax[i].position = backgroundStartPos[i];
            }
        //reset HeatherBundle
        if (tempPlayer)
        {
            Destroy(tempPlayer.gameObject);
        }

        if (tempProj)
        {
            Destroy(tempProj.gameObject);
        }

        if (tempWall)
        {
            Destroy(tempWall.gameObject);
        }

        tempProj = Instantiate(projPrefab, transform.position, Quaternion.identity);
        tempPlayer = Instantiate(playerPrefab, transform.position, Quaternion.identity);
        tempWall = Instantiate(wallPrefab, wallPos, Quaternion.identity);
        movingVCam.Follow = tempPlayer;
        //prevCamPos = movingVCam.transform.position;
        SwitchCamera();

    }

}
