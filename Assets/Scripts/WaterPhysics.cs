using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterPhysics : MonoBehaviour
{
    public float waterGravity = -0.1f; // Set positive to simulate buoyancy?
    public float accel = 0.4f;
    public float waterHeight = 0;
    public float waterDrag = 3;

    private Vector2 velocity;
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (transform.position.y > waterHeight)
        {
            rb.gravityScale = 1;
            rb.drag = 0;
        }
        else if (Mathf.Sign(transform.position.y) < 0.1f && rb.velocity.magnitude < 0.1f)
        {
            
        }
        else
        {
            rb.gravityScale = 0;
            rb.drag = waterDrag;
            rb.AddForce(Vector2.up * waterGravity * 100);
        }
    }
}
