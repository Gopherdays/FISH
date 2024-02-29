using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish : MonoBehaviour
{
    public float str; // how hard the fish pulls back
    public float pauseBase; // the minimum amount of time the fish pauses between pulls
    public float pauseExtra; // the maximum amount of random extra time that can be added
    public float pullTime; // the amount of time the fish tugs on the line
    public float pullExtra; // the maximum amount of random extra time
    public float time; // the amount of time you have to reel in the fish before it automatically escapes

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
