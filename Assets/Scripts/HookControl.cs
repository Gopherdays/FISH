using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HookControl : MonoBehaviour
{
    public PlayerStatsEpic stats;

    Rigidbody2D rb;
    CameraScript cs;
    public Transform origin;
    public float hookSpeedHorizontal = 2;
    public float hookSpeedVertical = 4;
    public bool thrown = false;
    public bool fishing;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        cs = GameObject.Find("Virtual Camera").GetComponent<CameraScript>();
        hookSpeedHorizontal = 2 * stats.lineSpeedHorizontal;
        hookSpeedVertical = 4 * stats.lineSpeedVertical;
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
            rb.velocity = Vector2.zero;
            if (Input.GetMouseButtonDown(0))
                ThrowHook();
        }
        
    }

    void ThrowHook()
    {
        rb.AddForce(new Vector2(Random.Range(0.25f, 1.25f) * -300, Random.Range(0.5f, 1.25f) * 300));
        thrown = true;
        fishing = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Fish"))
        {
            Debug.Log("Catch this fish: " + collision.gameObject.name);
            cs.Shake(100);
        }
    }

    public void Up(InputAction.CallbackContext context)
    {

    }

    public void Down(InputAction.CallbackContext context)
    {

    }

    public void Left(InputAction.CallbackContext context)
    {

    }

    public void Right(InputAction.CallbackContext context)
    {

    }
}
