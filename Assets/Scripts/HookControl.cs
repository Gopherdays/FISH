using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookControl : MonoBehaviour
{
    public PlayerStatsEpic stats;

    Rigidbody2D rb;
    public Transform origin;
    public float hookSpeedHorizontal = 2;
    public float hookSpeedVertical = 8;
    public bool thrown = false;
    public bool fishing;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        hookSpeedHorizontal = 2 * stats.lineSpeedHorizontal;
        hookSpeedVertical = 8 * stats.lineSpeedVertical;
        fishing = false;
    }

    void Update()
    {
        if (thrown)
        {
            if (rb.position.y < 0)
                rb.AddForce(new Vector2(Input.GetAxis("Horizontal") * hookSpeedHorizontal, Input.GetAxis("Vertical") * hookSpeedVertical));
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
        rb.AddForce(new Vector2(Random.Range(0.25f, 1.25f) * -500, Random.Range(0.5f, 1.25f) * 1000));
        thrown = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Fish"))
        {
            Debug.Log("Catch this fish: " + collision.gameObject.name);
            fishing = true;
            collision.gameObject.
            GameObject.Find("Virtual Camera").GetComponent<CameraScript>().Shake(100);
        }
    }
}
