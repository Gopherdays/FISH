using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clouds : MonoBehaviour
{
    public Sprite[] clouds;
    private Rigidbody2D rb;
    private float random;

    private void FixedUpdate()
    {
        if (rb == null)
        {
            rb = GetComponent<Rigidbody2D>();
            GetComponent<SpriteRenderer>().sprite = clouds[Random.Range(0, clouds.Length)];
            GetComponent<SpriteRenderer>().sortingOrder = Random.Range(-110, -105);
            random = Random.Range(0.2f, 0.6f);
        }
        rb.velocity = Vector2.left * random;
        if (transform.localPosition.x < -12)
        {
            if (Random.value > 0.5f)
                Destroy(gameObject);
            else
                transform.position.Set(Random.Range(10, 14f), Random.Range(3, 6f), transform.position.z);
        }
    }
}
