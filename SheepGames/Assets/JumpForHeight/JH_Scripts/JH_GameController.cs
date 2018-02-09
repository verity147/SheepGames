using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class JH_GameController : MonoBehaviour {


    public Transform projPrefab;
    public Transform playerPrefab;
    public Transform wallPrefab;
    public GameObject trajPointPrefab;
    public Transform[] toBeMovedInParallax;
    public Camera mCam; /// main camera
    public CinemachineVirtualCameraBase staticVCam;
    public CinemachineVirtualCameraBase movingVCam;
    public float smoothFactor;

    private int numberOfTrajPoints = 30;
    private Vector2 jumpForce;
    private Vector3 wallPos;
    private Vector3 prevCamPos;
    private Vector3 projSpawnPos;
    private Transform tempProj;
    private Transform tempWall;
    private Transform tempPlayer;
    internal Transform[] playerParts;
    internal Transform[] wallChildren;
    private List<Vector3> backgroundStartPos;
    private List<GameObject> trajectoryPoints;
    private List<float> parallaxMag;

    /// Parallax magnitude

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
        ///fill trajectoryPoints List with objects to display & turn off their renderer
        trajectoryPoints = new List<GameObject>();
        for(int i = 0; i < numberOfTrajPoints; i++)
        {
            GameObject dot = Instantiate(trajPointPrefab);
            dot.GetComponent<SpriteRenderer>().enabled = false;
            trajectoryPoints.Insert(i, dot);
        }
        ///Projectile self destroys when it gets to the right of Start so its position is set to ensure it always spawns left
        projSpawnPos = transform.position - new Vector3(0.01f, 0f, 0f);

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

    private void OnMouseDrag()
    {
        if (tempProj != null)
        {
            jumpForce = tempProj.gameObject.GetComponent<SpringJoint2D>().GetReactionForce(Time.time);
            //float angle = Mathf.Atan2(jumpForce.y, jumpForce.x) * Mathf.Rad2Deg;
            DrawTrajectoryPoints()
        }
    }

    internal void ChangeCollision(Transform[] objectToChange, int layerIndex)
    {
        ///gets called to change wall collision, player collision
        foreach (Transform child in objectToChange)
        {
            child.gameObject.layer = layerIndex;
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

        tempProj = Instantiate(projPrefab, projSpawnPos, Quaternion.identity);
        tempPlayer = Instantiate(playerPrefab, playerPrefab.transform.position, Quaternion.identity);
        tempWall = Instantiate(wallPrefab, wallPos, Quaternion.identity);
        wallChildren = tempWall.GetComponentsInChildren<Transform>();
        playerParts = tempPlayer.GetComponentsInChildren<Transform>();
        movingVCam.Follow = tempPlayer;
        staticVCam.MoveToTopOfPrioritySubqueue();
    }

}
