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

    GameObject lossScreen;

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

    private GameObject _sightCone;
    private bool _canBlindfold = false;
    private float _blindfoldedTime = 0;

    void Start()
    {
        guardState = GuardState.Patrol;
        playerObject = GameObject.FindGameObjectWithTag("Player");
        player = playerObject.GetComponent<PlayerBehavior>();
        _sightCone = this.transform.Find("GuardSightCone").gameObject;

        targetIndex = 0;
        SetNextWaypoint();


        var canvas = GameObject.FindGameObjectWithTag("Canvas");
        if (canvas != null)
            if (canvas.transform.Find("LevelLost") != null)
                lossScreen = canvas.transform.Find("LevelLost").gameObject;

    }

    private void Update()
    {
        if (this._canBlindfold && guardState != GuardState.Blindfolded)
        {
            if (this.player.pickup?.itemName == "Blindfold")
            {
                if (Input.GetKeyDown(KeyCode.J) || Input.GetKeyDown(KeyCode.JoystickButton0))
                {
                    this.guardState = GuardState.Blindfolded;
                    this.spriteRenderer.color = Color.magenta;
                    this._blindfoldedTime = 500;
                    this.player.pickup = null;
                }
            }
        }
        
    }

    void FixedUpdate()
    {
        switch (guardState)
        {
            case GuardState.Blindfolded:
                if (this._blindfoldedTime > 0)
                {
                    _sightCone.SetActive(false);
                    this._blindfoldedTime--;
                    rb.velocity = Vector2.zero;
                }
                else
                {
                    _sightCone.SetActive(true);
                    this.guardState = GuardState.Patrol;
                }
                break; // Stop patrolling
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
                var alertness = CheckPlayerAlertness();
                Debug.Log(alertness);
                if (alertness > 0)
                {
                    suspicionCooldown = 0;
                    suspicionAlert += alertness * Time.deltaTime;
                }
                else
                {
                    if (suspicionAlert > 0)
                        suspicionAlert -= Time.deltaTime;
                    else if (suspicionAlert < 0)
                        suspicionAlert = 0;
                    suspicionCooldown += Time.deltaTime;
                }
                break;
            case GuardState.Chase:
                spriteRenderer.color = Color.red;
                player.SetCanMove(false);
                lossScreen.SetActive(true);
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
            if (CheckPlayerAlertness() > 0)
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

    float CheckPlayerAlertness()
    {
        var pathToPlayer = playerObject.GetComponent<Rigidbody2D>().position - rb.position;
        if (pathToPlayer.magnitude > suspicionRange) { return 0; }
        var viewAngle = Vector3.Angle(pathToPlayer.normalized, rb.transform.up);
        if (Math.Abs(viewAngle) > suspicionAngle) { return 0; }

        //TODO raytrace

        if (pathToPlayer.magnitude == 0) { return float.PositiveInfinity; }
        else { return (suspicionRange * suspicionRange) / (pathToPlayer.magnitude * pathToPlayer.magnitude); }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == player.gameObject)
        {
            this._canBlindfold = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject == player.gameObject)
        {
            this._canBlindfold = false;
        }
    }

    enum GuardState
    {
        Patrol,
        Suspicious,
        Chase,
        Blindfolded,
        Stunned,
        Reset,
    }
}
