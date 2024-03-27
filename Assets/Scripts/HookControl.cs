using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class HookControl : MonoBehaviour
{
    public PlayerStatsEpic stats;

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

    void Start()
    {
        player = GameObject.Find("Player");
        cam = GameObject.Find("Virtual Camera").GetComponent<CinemachineVirtualCamera>();
        rb = GetComponent<Rigidbody2D>();
        cs = cam.gameObject.GetComponent<CameraScript>();
        fr = GameObject.Find("Main Camera").GetComponent<FishingRod>();
        fl = GameObject.Find("Line Renderer").GetComponent<FishingLine>();
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
            cam.Follow = player.transform;
            transform.position = player.transform.position;
            fish.transform.rotation = Quaternion.Euler (0 , 0 , 90);
            fish.transform.position = new Vector2(-7, -5);
            fish.GetComponent<SpriteRenderer>().sortingOrder = 420; //lol
            fl.hook = fish;
            cs.Shake(100);
            fr.enabled = true;
            enabled = false;
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
