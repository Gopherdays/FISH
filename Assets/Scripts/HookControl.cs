using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookControl : MonoBehaviour
{
    Rigidbody2D rb;
    public Transform origin;
    public float hookMovementSpeed;
    public bool thrown = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Physics2D.IgnoreCollision(GetComponent<CircleCollider2D>(), origin.gameObject.GetComponent<BoxCollider2D>());
    }

    void Update()
    {
        if (thrown)
        {
            if (rb.position.y < 0)
                rb.AddForce(new Vector2(Input.GetAxis("Horizontal") / 2, Input.GetAxis("Vertical") * 2) * hookMovementSpeed);
        }
        else
        {
            transform.position = origin.transform.position;
            if (Input.GetMouseButtonDown(0))
                ThrowHook();
        }
        
    }

    void ThrowHook()
    {
        Physics2D.IgnoreCollision(GetComponent<CircleCollider2D>(), origin.gameObject.GetComponent<BoxCollider2D>());
        rb.AddForce(new Vector2(Random.Range(0.25f, 1.25f) * -500, Random.Range(0.5f, 1.25f) * 1000));
        thrown = true;
    }
}
