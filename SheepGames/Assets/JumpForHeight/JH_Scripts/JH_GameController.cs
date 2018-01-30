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
    public float smoothFactor;


    private Vector3 wallPos;
    private Vector3 prevCamPos;
    private Transform tempProj;
    private Transform tempWall;
    private Transform tempPlayer;
    private Transform[] wallChildren;
    private List<Vector3> backgroundStartPos;
    private List<float> parallaxMag;

    //use this script to retain trail from previous tries
    //determine which wall gets used in which level

    private void Start()
    {
        wallPos = wallPrefab.position;
        prevCamPos = movingVCam.transform.position;

        backgroundStartPos = new List<Vector3>();
        parallaxMag = new List<float>();
        foreach(Transform obj in toBeMovedInParallax)
            {
                backgroundStartPos.Add(obj.position);
                parallaxMag.Add(smoothFactor);
                smoothFactor *= 0.3f;
            }
        SpawnNewPlayer();
    }


    private void LateUpdate()
    {
        float camDist = 0f;
        if(mCam.GetComponent<CinemachineBrain>().ActiveVirtualCamera.Name == movingVCam.Name)
        {
            camDist = Mathf.Abs(movingVCam.transform.position.x - prevCamPos.x);
            for (int i = 0; i < toBeMovedInParallax.Length; i++)
            {
                float nextX = toBeMovedInParallax[i].position.x - camDist; //greater z equals smaller nextX
                toBeMovedInParallax[i].position = new Vector3 (Mathf.Lerp(toBeMovedInParallax[i].position.x, nextX, parallaxMag[i]*Time.deltaTime),
                                                  toBeMovedInParallax[i].position.y,
                                                  toBeMovedInParallax[i].position.z);
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

    internal void ChangeWallCollision()
    {
        //better to trigger from player animationEvent
        ///changes collision to !CollidePlayer for all child elements
        foreach (Transform child in wallChildren)
        {
            child.gameObject.layer = 9;
        }
    }

    public void SpawnNewPlayer()
    {
        ///reset backgrounds
       for(int i = 0; i < toBeMovedInParallax.Length; i++)
            {
                toBeMovedInParallax[i].position = backgroundStartPos[i];
            }
        ///Heatherbundle is reset as part of Wall
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
        tempPlayer = Instantiate(playerPrefab, playerPrefab.transform.position, Quaternion.identity);
        tempWall = Instantiate(wallPrefab, wallPos, Quaternion.identity);
        wallChildren = tempWall.GetComponentsInChildren<Transform>();
        movingVCam.Follow = tempPlayer;
        staticVCam.MoveToTopOfPrioritySubqueue();

    }

}
