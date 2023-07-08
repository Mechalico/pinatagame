using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guard : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
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

    Vector2 locationStopped;
    float rotationStopped;

    public float suspicionRange = 10;
    public float suspicionAngle = 30;
    public float suspicionAlertLimit = 1f;
    float suspicionAlert = 0;
    public float suspicionCooldownLimit = 3f;
    float suspicionCooldown = 0;

    void Start()
    {
        guardState = GuardState.Patrol;
        playerObject = GameObject.FindGameObjectWithTag("Player");
        player = playerObject.GetComponent<PlayerBehavior>();

        targetIndex = 0;
        SetNextWaypoint();
    }

    void FixedUpdate()
    {
        switch (guardState)
        {
            case GuardState.Patrol:
                spriteRenderer.color = Color.blue;
                timeWalked += Time.deltaTime;
                rb.velocity = inputMovement / timeToArrive;
                rb.rotation += (rotationSpeed * Time.deltaTime);
                //move in that direction
                if (timeWalked >= timeToArrive)
                {
                    SetNextWaypoint();
                }
                break;
            case GuardState.Suspicious:
                rb.velocity = Vector2.zero;
                spriteRenderer.color = Color.yellow;
                if (CheckPlayerInRange())
                {
                    suspicionCooldown = 0;
                    suspicionAlert += Time.deltaTime;
                }
                else
                {
                    suspicionAlert = 0;
                    suspicionCooldown += Time.deltaTime;
                }
                break;
            case GuardState.Chase:
                spriteRenderer.color = Color.red;
                break;
        }

        CheckForStateChanges();
    }

    void SetNextWaypoint()
    {
        var thisWaypoint = targets[targetIndex];
        rb.position = thisWaypoint.transform.position;
        rb.rotation = thisWaypoint.transform.rotation.eulerAngles.z;

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

            var rawRotation = nextWaypoint.transform.rotation.eulerAngles.z - rb.rotation;

            if (rawRotation < 0)
            {
                if (nextWaypointScript.isClockwise)
                    rotationSpeed = rawRotation / nextWaypointScript.timeToArrive;
                else
                    rotationSpeed = (rawRotation + 360) / nextWaypointScript.timeToArrive;
            }
            else if (rawRotation > 0)
            {
                if (nextWaypointScript.isClockwise)
                    rotationSpeed = (rawRotation - 360) / nextWaypointScript.timeToArrive;
                else
                    rotationSpeed = rawRotation / nextWaypointScript.timeToArrive;
            }
            else if (rawRotation == 0)
                rotationSpeed = 0;
        }
    }

    void CheckForStateChanges()
    {
        if (guardState == GuardState.Patrol || guardState == GuardState.Reset)
        {
            if (CheckPlayerInRange())
            {
                suspicionCooldown = 0;
                suspicionAlert = 0;
                guardState = GuardState.Suspicious;
            }
        }
        if (guardState == GuardState.Suspicious)
        {
            if (suspicionAlert > suspicionAlertLimit) guardState = GuardState.Chase;
            if (suspicionCooldown > suspicionCooldownLimit) guardState = GuardState.Patrol;
        }
    }

    bool CheckPlayerInRange()
    {
        var pathToPlayer = playerObject.GetComponent<Rigidbody2D>().position - rb.position;
        Debug.Log(pathToPlayer.magnitude);
        if (pathToPlayer.magnitude > suspicionRange) { return false; }
        var angle = Vector3.Angle(pathToPlayer.normalized, rb.transform.up);
        Debug.Log(angle);
        if (Math.Abs(angle) > suspicionRange) { return false; }

        //TODO raytrace

        return true;
    }

    enum GuardState
    {
        Patrol,
        Suspicious,
        Chase,
        Stunned,
        Reset
    }
}
