using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour
{
    public float moveSpeed;
    public float rollSpeed;

    public float heldCandy;

    public Rigidbody2D rb;

    Vector2 inputMovement;

    public int pickup; //could replace with enum?

    void Update()
    {
        inputMovement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    }

    void FixedUpdate()
    {
        float candyPenalty = 1 - Mathf.Min(heldCandy / 1250, 0.8f);
        rb.velocity = moveSpeed * candyPenalty * Time.deltaTime * inputMovement;
    }

    public void AddCandy(float candyToAdd)
    {
        heldCandy += candyToAdd;
        
    }
}
