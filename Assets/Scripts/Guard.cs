using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guard : MonoBehaviour
{
    GameObject playerObject;
    PlayerBehavior player;

    public Rigidbody2D rb;

    public GameObject[] targets;
    int targetIndex;

    GuardState guardState;

    Vector2 inputMovement;

    float timeToArrive;
    float timeWalked;
    float rotationSpeed;
    Vector3 rotationVelocity;

    void Start()
    {
        guardState = GuardState.Patrol;
        playerObject = GameObject.FindGameObjectWithTag("Player");
        player = playerObject.GetComponent<PlayerBehavior>();

        targetIndex = 0;
        setNextWaypoint();
    }

    void FixedUpdate()
    {
        if (guardState == GuardState.Patrol)
        {
            timeWalked += Time.deltaTime;
            rb.velocity = inputMovement / timeToArrive;
            rb.rotation += (rotationSpeed * Time.deltaTime);
            //move in that direction
            if (timeWalked >= timeToArrive)
            {
                setNextWaypoint();
            }

        }

    }

    void setNextWaypoint()
    {
        var thisWaypoint = targets[targetIndex];
        rb.position = thisWaypoint.transform.position;
        rb.rotation = thisWaypoint.transform.rotation.eulerAngles.z;

        Debug.Log(thisWaypoint.transform.rotation.eulerAngles.z);
        Debug.Log("Set next waypoint");

        targetIndex++;
        timeWalked = 0;
        //target initial point after reaching target point
        if (targetIndex >= targets.Length) targetIndex = 0;

        var nextWaypoint = targets[targetIndex];
        var nextWaypointScript = nextWaypoint.GetComponent<WaypointScript>();

        //get a vector pointing to your next waypoint.
        //this tells you direction and distance
        inputMovement = (Vector2)nextWaypoint.transform.position - rb.position;

        if (nextWaypointScript.timeToArrive > 0)
        {
            timeToArrive = nextWaypointScript.timeToArrive;

            if (nextWaypoint.transform.rotation.eulerAngles.z == rb.rotation)
                rotationSpeed = 0;
            else
            {
                if (nextWaypointScript.isClockwise)
                    rotationSpeed = (nextWaypoint.transform.rotation.eulerAngles.z - rb.rotation) / nextWaypointScript.timeToArrive;
                else
                    rotationSpeed = (nextWaypoint.transform.rotation.eulerAngles.z - rb.rotation - 360) / nextWaypointScript.timeToArrive;
            }

            Debug.Log(rotationSpeed);
        }
    }

    enum GuardState
    {
        Patrol,
        Chase,
        Stunned,
        Reset
    }
}
