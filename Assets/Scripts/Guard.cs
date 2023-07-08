using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guard : MonoBehaviour
{
    public PlayerBehavior player;
    public float speed;

    public Rigidbody2D rb;

    public Transform[] targets;
    public int targetIndex;

    void FixedUpdate()
    {
        //turn around after reaching the last one
        if (targetIndex >= targets.Length) targetIndex = 0;

        //get a vector pointing to your next waypoint.
        //this tells you direction and distance
        Vector2 inputMovement = targets[targetIndex].position - transform.position;

        //if guards should smoothly turn around, that code would go here

        //move in that direction
        rb.velocity = inputMovement.normalized * speed * Time.deltaTime;
        //when you reach a waypoint, turn around.
        if (inputMovement.magnitude < 0.1) targetIndex += 1;
        
    }
}
