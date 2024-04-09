using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish : MonoBehaviour
{
    Rigidbody2D rb;
    Vector2 velocityVector;
    public float swimSpeed;
    public float swimCycle = 0.2f;
    public Vector2 hookPoint;

    public int value;
    public int points;
    public string description;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        velocityVector = rb.velocity;
        velocityVector.x = swimSpeed;
        rb.velocity = velocityVector;
    }
}
