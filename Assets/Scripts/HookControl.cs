using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookControl : MonoBehaviour
{
    Rigidbody2D rb;
    public float hookMovementSpeed;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (rb.position.y < 0)
            rb.AddForce(new Vector2(Input.GetAxis("Horizontal") / 2, Input.GetAxis("Vertical") * 2) * hookMovementSpeed);
    }
}
