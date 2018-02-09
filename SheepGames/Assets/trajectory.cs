using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CannonScript : MonoBehaviour
{
    //Script goes on cannon object

    // TrajectoryPoint and Ball will be instantiated
    public GameObject TrajectoryPointPrefeb;
    public GameObject BallPrefb;

    private GameObject ball;
    private bool isPressed, isBallThrown;
    private float power = 25;
    private int numOfTrajectoryPoints = 30;
    private List<GameObject> trajectoryPoints;
    //---------------------------------------    
    void Start()
    {
        trajectoryPoints = new List<GameObject>();
        isPressed = isBallThrown = false;
        //   TrajectoryPoints are instatiated, fills list with points
        for (int i = 0; i < numOfTrajectoryPoints; i++)
        {
            GameObject dot = Instantiate(TrajectoryPointPrefeb);
            dot.GetComponent<Renderer>().enabled = false;
            trajectoryPoints.Insert(i, dot);
        }
    }
    //---------------------------------------    
    void Update()
    {
        if (isBallThrown)
            return;
        if (Input.GetMouseButtonDown(0))
        {
            isPressed = true;
            if (!ball)
                CreateBall();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isPressed = false;
            if (!isBallThrown)
            {
                ThrowBall();
            }
        }
        // when mouse button is pressed, cannon is rotated as per mouse movement and projectile trajectory path is displayed.
        if (isPressed)
        {
            Vector3 vel = GetForceFrom(ball.transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition));
            ///degree relative to x-axis
            float angle = Mathf.Atan2(vel.y, vel.x) * Mathf.Rad2Deg;
            ///degree converted to a rotation around z-axis
            transform.eulerAngles = new Vector3(0, 0, angle);
            SetTrajectoryPoints(transform.position, vel / ball.GetComponent<Rigidbody2D>().mass);
        }
        
    }
    //---------------------------------------    
    // Following method creates new ball
    //---------------------------------------    
    private void CreateBall()
    {
        ball = Instantiate(BallPrefb);
        Vector3 pos = transform.position; //@cannon position
        pos.z = 1;
        ball.transform.position = pos;
        ball.SetActive(false);
    }
    //---------------------------------------    
    // Following method gives force to the ball
    //---------------------------------------    
    private void ThrowBall()
    {
        ball.SetActive(true);
        ball.GetComponent<Rigidbody2D>().isKinematic = false;
        ball.GetComponent<Rigidbody2D>().AddForce(GetForceFrom(ball.transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition)), ForceMode2D.Impulse);
        isBallThrown = true;
    }
    //---------------------------------------    
    // Following method returns force by calculating distance between given two points
    //---------------------------------------    
    private Vector2 GetForceFrom(Vector3 fromPos, Vector3 toPos)
    {
        return (new Vector2(toPos.x, toPos.y) - new Vector2(fromPos.x, fromPos.y)) * power;
    }
    //---------------------------------------    
    // Following method displays projectile trajectory path. It takes two arguments, start position of object(ball) and initial velocity of object(ball).
    //---------------------------------------    
    void SetTrajectoryPoints(Vector3 pStartPosition, Vector3 pVelocity)
    {
        float velocity = Mathf.Sqrt((pVelocity.x * pVelocity.x) + (pVelocity.y * pVelocity.y));
        float angle = Mathf.Rad2Deg * (Mathf.Atan2(pVelocity.y, pVelocity.x));
        float fTime = 0;

        fTime += 0.1f;
        for (int i = 0; i < numOfTrajectoryPoints; i++)
        {
            float dx = velocity * fTime * Mathf.Cos(angle * Mathf.Deg2Rad);
            float dy = velocity * fTime * Mathf.Sin(angle * Mathf.Deg2Rad) - (Physics2D.gravity.magnitude * fTime * fTime / 2.0f);
            Vector3 pos = new Vector3(pStartPosition.x + dx, pStartPosition.y + dy, 2);
            trajectoryPoints[i].transform.position = pos;
            trajectoryPoints[i].GetComponent<Renderer>().enabled = true;
            trajectoryPoints[i].transform.eulerAngles = new Vector3(0, 0, Mathf.Atan2(pVelocity.y - (Physics.gravity.magnitude) * fTime, pVelocity.x) * Mathf.Rad2Deg);
            fTime += 0.1f;
        }
    }


    //--------------------------------------------------------------
    //--------------------------------------------------------------

    private int numberOfTrajectoryoPoint = 5;
    private GameObject[] trajectoryPointsSec;

    void Awake()
    {
        trajectoryPointsSec = new GameObject[numberOfTrajectoryoPoint];

        for (int i = 0; i < numberOfTrajectoryoPoint; i++)
        {
            trajectoryPoints[i] = Instantiate(TrajectoryPointPrefeb);
        }
    }
    public void DrawPath(Vector3 pStartPosition, Vector3 pVelocity)
    {
        Vector3 calculatedPosition;
        float fTime = 0;
        //fTime += 0.05f;
        //print("velocity at path drawer =" + pVelocity);

        for (int i = 0; i < numberOfTrajectoryoPoint; i++)
        {
            float dx = pVelocity.x * fTime;
            float dy = pVelocity.y * fTime - (Physics2D.gravity.magnitude * fTime * fTime / 2.0f);

            calculatedPosition = new Vector3(pStartPosition.x + dx, pStartPosition.y + dy, 2);

            trajectoryPoints[i].transform.position = calculatedPosition;

            fTime += 0.05f;
        }
    }
}