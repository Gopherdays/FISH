using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
using UnityEngine.UI;
using TMPro;

public class Fishing : MonoBehaviour
{
    bool up; // input variables
    bool left;
    bool down;
    bool right;
    bool confirm;
    bool changed;
    Vector2 temp = new Vector2();

    GameObject player; // Minigame variables
    public GameObject fish;
    public GameObject indicator;
    public GameObject catcher;
    public TextMeshProUGUI depthText;
    public GameObject[] things;
    Vector2 tempV;
    int catcherPos;
    int curCatcherPos;
    int indPos;
    int curIndPos;
    List<float> positions = new List<float>();

    public float speed; // Minigame stat variables
    public int turnChance;
    public int skipChance;
    public float rodStr;
    public float distance;
    float timer;
    bool direction;
    bool skip;

    public GameObject splash; // Hook mavement variables
    public Transform origin;
    public PlayerStatsEpic stats;
    Rigidbody2D rb;
    CameraScript cs;
    FishingLine fl;
    SpriteRenderer sr;
    CinemachineVirtualCamera cam;
    public float hookSpeedHorizontal = 2;
    public float hookSpeedVertical = 4;
    float prevY;
    bool thrown;

    GameObject fishingUI;
    GameObject depth;
    bool hooking; // mode swapping variables
    bool fishing;

    private void Start()
    {
        fishingUI = GameObject.Find("Fishing UI");
        fishingUI.SetActive(false);
        depth = GameObject.Find("Depth Meter");
        depth.SetActive(false);
        for (int i = 0; i < 8; i++) //loads the different positions on the indicator
        {
            positions.Add(i * 45);
        }
        indPos = Random.Range(2, 7);
        indicator.transform.rotation = Quaternion.Euler(0, 0, positions[indPos]);
        curIndPos = indPos;
        tempV = new Vector2(-7, -5);
        prevY = transform.position.y;
        player = GameObject.Find("Player");
        cam = GameObject.Find("Virtual Camera").GetComponent<CinemachineVirtualCamera>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        cs = cam.gameObject.GetComponent<CameraScript>();
        fl = GameObject.Find("Line Renderer").GetComponent<FishingLine>();
        hookSpeedHorizontal = 2 * stats.lineSpeedHorizontal;
        hookSpeedVertical = 4 * stats.lineSpeedVertical;
        temp = new Vector2(0, 0);
        fishing = false;
        up = false;
        left = false;
        down = false;
        right = false;
        hooking = true;
    }

    void Switch()
    {
        if (hooking)
        {
            Debug.Log("Catch this fish: " + fish.name);
            things = GameObject.FindGameObjectsWithTag("Fish");
            foreach (GameObject go in things)
            {
                if (go != fish)
                    go.SetActive(false);
            }
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
            fish.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
            distance = Vector2.Distance(fish.transform.position, player.transform.position);
            cam.Follow = player.transform;
            transform.position = player.transform.position;
            if (fish.transform.localScale.x > 0)
                fish.transform.SetPositionAndRotation(new Vector2(-7, -5), Quaternion.Euler(0, 0, 90));
            else if (fish.transform.localScale.x < 0)
                fish.transform.SetPositionAndRotation(new Vector2(-7, -5), Quaternion.Euler(0, 0, -90));
            fish.GetComponent<SpriteRenderer>().sortingOrder = 420; //lol
            fl.hook = fish;
            cs.Shake(100);
            sr.enabled = false;
            hooking = false;
            fishing = true;
            return;
        }
        else if (fishing)
        {
            StartCoroutine(WaitForAnimation());
        }
    }

    IEnumerator WaitForAnimation() // fire all of this code after you wait so the player gets to look at the fish
    {
        yield return new WaitForSeconds(1);
        print("Yay! You caught: " + fish.name.Remove(fish.name.Length - 7));
        fishingUI.SetActive(false);
        depth.SetActive(false);
        stats.bucket.Add(fish.GetComponent<Fish>().value);
        stats.points += fish.GetComponent<Fish>().points;
        Destroy(fish);
        foreach (GameObject go in things)
        {
            go.SetActive(true);
        }
        sr.enabled = true;
        fishing = false;
        hooking = true;
        GameObject.Find("Virtual Camera").GetComponent<CinemachineVirtualCamera>().Follow = GameObject.Find("Player").transform;
        fishingUI.gameObject.SetActive(true);
        depth.gameObject.SetActive(true);
        indPos = Random.Range(2, 7);
        catcherPos = 1;
        curCatcherPos = 1;
    }

    private void Update()
    {
        if (thrown && changed)
        {
            changed = false;
            switch (true) // figures out what direction the joystick is facing
            {
                case true when (up && !left && !right):
                    catcherPos = 0; // set the catcher indicator to the correct position based on the input values
                    temp = Vector2.up; // set the direction for hook movement
                    break;
                case true when (up && left):
                    catcherPos = 1;
                    temp.y = 1;
                    temp.x = -1;
                    break;
                case true when (left && !up && !down):
                    catcherPos = 2;
                    temp = Vector2.left;
                    break;
                case true when (down && left):
                    catcherPos = 3;
                    temp.y = -1;
                    temp.x = -1;
                    break;
                case true when (down && !left && !right):
                    catcherPos = 4;
                    temp = Vector2.down;
                    break;
                case true when (down && right):
                    catcherPos = 5;
                    temp.y = -1;
                    temp.x = 1;
                    break;
                case true when (right && !up && !down):
                    catcherPos = 6;
                    temp = Vector2.right;
                    break;
                case true when (up && right):
                    catcherPos = 7;
                    temp.y = 1;
                    temp.x = 1;
                    break;
                default:
                    temp = Vector2.zero;
                    break;
            }
        }
        else if (!thrown) // reset the hook position and start looking for a click to send the hook
        {
            if (transform.position != origin.transform.position)
                transform.position = origin.transform.position;
            if (rb.velocity != Vector2.zero)
                rb.velocity = Vector2.zero;
            if (confirm)
            {
                confirm = false;
                ThrowHook();
            }
        }
    }

    private void FixedUpdate()
    {
        if (hooking)
        {
            if (thrown && transform.position.y <= 0)
            {
                temp = temp.normalized;
                temp.y *= stats.lineSpeedVertical;
                temp.x *= stats.lineSpeedHorizontal;
                rb.AddForce(temp * 20);
            }
            prevY = transform.position.y;
        }

        else if (fishing)
        {
            if (timer < speed) // fish position update
                timer += Time.deltaTime;
            else
            {
                indPos = NewPos(indPos);
                timer = 0;
            }

            if (indPos != curIndPos) // fish indicator position update
            {
                indicator.transform.rotation = Quaternion.Euler(0, 0, positions[indPos]);
                curIndPos = indPos;
            }

            if (catcherPos != curCatcherPos) // catcher indicator position update
            {
                catcher.transform.rotation = Quaternion.Euler(0, 0, positions[catcherPos]);
                curCatcherPos = catcherPos;
            }

            if (catcherPos == indPos) // calculate where the fish wants to go
                distance -= rodStr * Time.deltaTime;

            else if (NewInt(catcherPos - 1) == indPos || NewInt(catcherPos + 1) == indPos)
                distance -= rodStr * Time.deltaTime * 0.5f;

            else
                distance += speed * Time.deltaTime * 3;
            
            depthText.text = (int)distance + "m";
            if (distance <= 0)
            {
                Switch();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Fish"))
        {
            fish = collision.gameObject;
            Switch();
        }
    }


    int NewPos(int position)
    {
        if (Random.Range(1, 101) <= turnChance)
            direction = !direction;

        if (Random.Range(1, 101) <= skipChance)
            skip = true;

        int temp;
        temp = position;
        if (direction && skip)
            temp += 2;
        else if (direction && !skip)
            temp++;
        else if (!direction && skip)
            temp -= 2;
        else
            temp--;
        temp = NewInt(temp);
        skip = false;
        return temp;
    }

    int NewInt(int temp)
    {
        switch (temp)
        {
            case 8:
                temp = 0;
                break;

            case 9:
                temp = 1;
                break;

            case -1:
                temp = 7;
                break;

            case -2:
                temp = 6;
                break;

            default:
                break;
        }
        return temp;
    }

    public void ThrowHook()
    {
        thrown = true;
        rb.AddForce(new Vector2(Random.Range(0.25f, 1.25f) * -300, Random.Range(0.5f, 1.25f) * 300));
    }

    // all of the input functions
    public void Up(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            up = true; // value that help track the joystick direction
            changed = true; // turn this value on to change the position of the catcher indicator
        }
        if (context.canceled)
        {
            up = false;
            changed = true;
        }
    }
    public void Left(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            left = true;
            changed = true;
        }
        if (context.canceled)
        {
            left = false;
            changed = true;
        }
    }
    public void Down(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            down = true;
            changed = true;
        }
        if (context.canceled)
        {
            down = false;
            changed = true;
        }
    }
    public void Right(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            right = true;
            changed = true;
        }
        if (context.canceled)
        {
            right = false;
            changed = true;
        }
    }
    public void A(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            confirm = true;
        }
    }
}
