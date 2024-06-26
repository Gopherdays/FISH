using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Serves both as the thing that controls the fish and holds its stats
public class Fish : MonoBehaviour
{
    Rigidbody2D rb;
    public float swimSpeed;
    public Vector2 hookPoint;

    public int value;
    public int points;
    public string description;
    public int volume;

    public float speed;
    public float escape;
    public int turnChance;
    public int skipChance;

    public int minDepth;
    public int maxDepth;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        rb.velocity = Vector2.right * swimSpeed;

        //Move to the opposite side of the screen if the fish goes past positive or negative 100 units
        if (Mathf.Abs(transform.position.x) > 100)
        {
            Vector3 temp = transform.position;
            temp.x *= -1;
            transform.position = temp;
        }
    }
}
