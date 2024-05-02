using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
using UnityEngine.UI;

public class HookControl : MonoBehaviour
{
    public GameObject splash;
    public PlayerStatsEpic stats;
    SpriteRenderer sr;
    GameObject player;
    CinemachineVirtualCamera cam;
    Rigidbody2D rb;
    CameraScript cs;
    FishingRod fr;
    FishingLine fl;
    GameObject[] things;
    public Transform origin;
    public float hookSpeedHorizontal = 2;
    public float hookSpeedVertical = 4;
    public bool thrown = false;
    public bool fishing;
    float prevY;
    Vector2 temp;
    bool up;
    bool down;
    bool left;
    bool right;

    void Start()
    {
        prevY = transform.position.y;
        player = GameObject.Find("Player");
        cam = GameObject.Find("Virtual Camera").GetComponent<CinemachineVirtualCamera>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        cs = cam.gameObject.GetComponent<CameraScript>();
        fr = GameObject.Find("Main Camera").GetComponent<FishingRod>();
        fl = GameObject.Find("Line Renderer").GetComponent<FishingLine>();
        hookSpeedHorizontal = 2 * stats.lineSpeedHorizontal;
        hookSpeedVertical = 4 * stats.lineSpeedVertical;
        temp = new Vector2(0,0);
        fishing = false;
        up = false;
        left = false;
        down = false;
        right = false;
    }
    void Update()
    {
        if (thrown)
        {
            switch (true)
            {
                case true when (up && !left && !right):
                    temp = Vector2.up;
                    break;
                case true when (up && left):
                    temp.y = 1;
                    temp.x = -1;
                    break;
                case true when (left && !up && !down):
                    temp = Vector2.left;
                    break;
                case true when (down && left):
                    temp.y = -1;
                    temp.x = -1;
                    break;
                case true when (down && !left && !right):
                    temp = Vector2.down;
                    break;
                case true when (down && right):
                    temp.y = -1;
                    temp.x = 1;
                    break;
                case true when (right && !up && !down):
                    temp = Vector2.right;
                    break;
                case true when (up && right):
                    temp.y = 1;
                    temp.x = 1;
                    break;
                default:
                    temp = Vector2.zero;
                    break;
            }
        }
        else
        {
            transform.position = origin.transform.position;
            rb.velocity = Vector2.zero;
            if (Input.GetMouseButtonDown(0))
                ThrowHook();
        }
    }
    private void FixedUpdate()
    {
        if (transform.position.y < 0 && prevY >= 0)
        {
            for (int i = 0; i < Random.Range(50, 80); i++)
            {
                GameObject g = Instantiate(splash, transform.position, Quaternion.identity);
                float xVal = Random.Range(-0.5f, 0.5f);
                g.GetComponent<Rigidbody2D>().AddForce(new Vector2(xVal, 2 + Random.Range(0f,1f) - Mathf.Abs(2 * xVal)) * (75 + Random.Range(0,75)));
                Destroy(g, 3);
            }
        }
        if (thrown && transform.position.y <= 0)
        {
            temp = temp.normalized;
            temp.y *= stats.lineSpeedVertical;
            temp.x *= stats.lineSpeedHorizontal;
            rb.AddForce(temp * 20);
        }
        prevY = transform.position.y;
    }
    public void ThrowHook()
    {
        rb.AddForce(new Vector2(Random.Range(0.25f, 1.25f) * -300, Random.Range(0.5f, 1.25f) * 300));
        thrown = true;
        fishing = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Fish"))
        {
            GameObject fish = collision.gameObject;
            Debug.Log("Catch this fish: " + fish.name);
            things = GameObject.FindGameObjectsWithTag("Fish");
            foreach (GameObject go in things)
            {
                if (go != fish)
                    go.SetActive(false);
            }
            fr.things = things;
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
            fr.hook = this.gameObject;
            fish.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
            fr.distance = Vector2.Distance(fish.transform.position, player.transform.position);
            fr.fish = fish;
            fr.fl = fl;
            fr.sr = sr;
            cam.Follow = player.transform;
            transform.position = player.transform.position;
            fish.transform.SetPositionAndRotation(new Vector2(-7, -5), Quaternion.Euler (0 , 0 , 90));
            fish.GetComponent<SpriteRenderer>().sortingOrder = 420; //lol
            fl.hook = fish;
            cs.Shake(100);
            fr.enabled = true;
            sr.enabled = false;
            enabled = false;
        }
    }

    public void Up(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            up = true;
        }
        else if (context.canceled)
        {
            up = false;
        }
    }

    public void Down(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            down = true;
        }
        else if (context.canceled)
        {
            down = false;
        }
    }

    public void Left(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            left = true;
        }
        else if (context.canceled)
        {
            left = false;
        }
    }

    public void Right(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            right = true;
        }
        else if (context.canceled)
        {
            right = false;
        }
    }
}
