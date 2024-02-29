using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterPhysics : MonoBehaviour
{
    public float waterGravity = -0.1f; // Set positive to simulate buoyancy?
    public float waterHeight = 0;
    public float waterDrag = 3;

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
        else
        {
            rb.gravityScale = 0.15f * Mathf.Sign(rb.velocity.y - waterGravity);
            rb.drag = waterDrag;
        }
    }
}
