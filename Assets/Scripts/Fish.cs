using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish : MonoBehaviour
{
    public float speed;
    public int randomness;

    WaterPhysics waterPhysics;
    Rigidbody2D rb;
    Vector2 velocityVector;
    public float swimSpeed;
    public float swimCycle = 0.2f;
    float randomOffset;
    public float bounceSpeed = 4;
    public Vector2 hookPoint;

    void Start()
    {
        waterPhysics = GetComponent<WaterPhysics>();
        rb = GetComponent<Rigidbody2D>();
        randomOffset = Random.Range(0, Mathf.PI * 2);
    }

    void Update()
    {
        waterPhysics.waterGravity = Mathf.Sin(randomOffset + Time.time * bounceSpeed) * swimCycle;
        velocityVector = rb.velocity;
        velocityVector.x = swimSpeed;
        rb.velocity = velocityVector;
    }
}
