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

    public int pickup;

    void Update()
    {
        inputMovement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    }

    void FixedUpdate()
    {
        rb.velocity = inputMovement * moveSpeed * Time.deltaTime;
    }
}
