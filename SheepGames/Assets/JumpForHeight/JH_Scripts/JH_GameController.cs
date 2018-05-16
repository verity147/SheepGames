using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;

public class JH_GameController : MonoBehaviour {


    public Transform playerPrefab;
    public Transform wallPrefab;
    public GameObject trajPointPrefab;
    public Transform[] toBeMovedInParallax;
    public Camera mCam; /// main camera
    public CinemachineVirtualCameraBase staticVCam;
    public CinemachineVirtualCameraBase movingVCam;
    public float smoothFactor;

    private int numberOfTrajPoints = 4;
    private Vector2 playerJumpForce;
    private Vector3 wallPos;
    private Vector3 prevCamPos;
    private Vector3 projSpawnPos;
    private Transform tempWall;
    private Transform tempPlayer;
    internal Transform[] playerParts;
    internal Transform[] wallChildren;
    internal SpectatorHandler[] spectators;
    private Transform trajectoryPointsHolder;
    private List<Vector3> backgroundStartPos;
    private List<GameObject> trajectoryPoints;
    private List<float> parallaxMag;    /// Parallax magnitude
    internal bool drawNow = false;
    private Vector2[] trajPositions;

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

        ///this is only to organize the hierarchy
        trajectoryPointsHolder = transform.Find("TrajectoryPointsHolder").transform;
        ///fill trajectoryPoints List with objects to display & turn off their renderer
        trajectoryPoints = new List<GameObject>();
        for(int i = 0; i < numberOfTrajPoints; i++)
        {
            GameObject dot = Instantiate(trajPointPrefab, trajectoryPointsHolder);
            dot.GetComponent<SpriteRenderer>().enabled = false;
            trajectoryPoints.Insert(i, dot);
        }

        spectators = FindObjectsOfType<SpectatorHandler>();

        SpawnNewPlayer();
    }

    internal void DrawTrajectoryPoints(Vector3 fromPos, Vector3 pointVelocity)
    {
        float pointVelRoot = Mathf.Sqrt((pointVelocity.x * pointVelocity.x) + (pointVelocity.y * pointVelocity.y));
        float angle = Mathf.Rad2Deg * (Mathf.Atan2(pointVelocity.y, pointVelocity.x));
        float count = 0f;
        count += 0.1f;
        for (int i = 0; i < numberOfTrajPoints; i++)
        {
            float dx = pointVelRoot * count * Mathf.Cos(angle * Mathf.Deg2Rad);
            float dy = pointVelRoot * count * Mathf.Sin(angle * Mathf.Deg2Rad) - (Physics2D.gravity.magnitude * count * count / 2.0f);
            Vector2 pos = new Vector2(fromPos.x + dx, fromPos.y + dy);
            trajectoryPoints[i].transform.position = pos;
            trajectoryPoints[i].GetComponent<SpriteRenderer>().enabled = true;
            //does the following rotate the individual points arounf their own z only? A: Yep.
            trajectoryPoints[i].transform.eulerAngles = new Vector3(0, 0, Mathf.Atan2(pointVelocity.y - (Physics.gravity.magnitude) * count, pointVelocity.x) * Mathf.Rad2Deg);
            count += 0.1f;
        }
    }

    private void LateUpdate()
    {
        float camDist = 0f;
        if(mCam.GetComponent<CinemachineBrain>().ActiveVirtualCamera.Name == movingVCam.Name)
        {
            camDist = Mathf.Abs(movingVCam.transform.position.x - prevCamPos.x);
            for (int i = 0; i < toBeMovedInParallax.Length; i++)
            {
                float nextX = toBeMovedInParallax[i].position.x - camDist; ///greater z equals smaller nextX
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

    internal void ChangeCollision(Transform[] objectToChange, int layerIndex)
    {
        ///gets called to change wall collision
        foreach (Transform child in objectToChange)
        {
            child.gameObject.layer = layerIndex;
        }
    }    

    public void SpawnNewPlayer()
    {
        HideTrajectory();
        ///stop the current reation animation from all spectators
        foreach (SpectatorHandler spectator in spectators)
        {
            spectator.StopAllCoroutines();
        }
        ///reset backgrounds
        for (int i = 0; i < toBeMovedInParallax.Length; i++)
        {
            toBeMovedInParallax[i].position = backgroundStartPos[i];
        }
        if (tempPlayer)
        {
            Destroy(tempPlayer.gameObject);
        }
        ///Heatherbundle is reset as part of Wall
        if (tempWall)
        {
            Destroy(tempWall.gameObject);
        }
        tempPlayer = Instantiate(playerPrefab, playerPrefab.transform.position, Quaternion.identity, transform);
        tempWall = Instantiate(wallPrefab, wallPos, Quaternion.identity);
        wallChildren = tempWall.GetComponentsInChildren<Transform>();
        playerParts = tempPlayer.GetComponentsInChildren<Transform>();
        movingVCam.Follow = tempPlayer;
        staticVCam.MoveToTopOfPrioritySubqueue();
    }

    public void HideTrajectory()
    {
        ///hide trajectory points
        foreach (GameObject point in trajectoryPoints)
        {
            point.GetComponent<SpriteRenderer>().enabled = false;
        }
    }
}
