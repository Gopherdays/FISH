using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterPhysics : MonoBehaviour
{
    public float waterGravity = -0.1f; // Set positive to simulate buoyancy?
    public float waterHeight = 0;
    public float waterDrag = 3;
    public bool above; // constantly setting the gravity scale and drag is messing with my code so I made it only set once when it changes

    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (transform.position.y > waterHeight && !above)
        {
            above = true;
            rb.gravityScale = 1;
            rb.drag = 0;
        }
        else if (transform.position.y < waterHeight && above)
        {
            above = false;
            rb.gravityScale = 0.15f * Mathf.Sign(rb.velocity.y - waterGravity);
            rb.drag = waterDrag;
        }
    }
}
