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
    bool changed;
    Vector2 temp = new Vector2();

    GameObject player; // Minigame variables
    public GameObject fish;
    public GameObject indicator;
    public GameObject catcher;
    public GameObject hook;
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
    bool reset;

    private void Start()
    {
        GameObject.Find("Fishing UI").SetActive(false);
        GameObject.Find("Depth Meter").SetActive(false);
        for (int i = 0; i < 8; i++) //loads the different positions on the indicator
        {
            positions.Add(i * 45);
        }
        indPos = Random.Range(2, 7);
        indicator.transform.rotation = Quaternion.Euler(0, 0, positions[indPos]);
        curIndPos = indPos;
        rb = GetComponent<Rigidbody2D>();
    }

    void Switch()
    {

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
        else // reset the hook position and start looking for a click to send the hook
        {
            if (transform.position != origin.transform.position)
                transform.position = origin.transform.position;
            if (rb.velocity != Vector2.zero)
                rb.velocity = Vector2.zero;
            if (Input.GetMouseButtonDown(0))
                ThrowHook();
        }
    }

    private void FixedUpdate()
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
    }

    IEnumerator WaitForAnimation() // fire all of this code after you wait so the player gets to look at the fish
    {
        reset = false;
        yield return new WaitForSeconds(1);
        print("Yay! You caught: " + fish.name.Remove(fish.name.Length - 7));
        GameObject.Find("Fishing UI").SetActive(false);
        GameObject.Find("Depth Meter").SetActive(false);
        GameObject.Find("Virtual Camera").GetComponent<CameraScript>().cast = false;
        fl.hook = hook;
        HookControl hooky = hook.GetComponent<HookControl>();
        hooky.ThrowHook();
        hooky.enabled = true;
        hook.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
        hook.SetActive(true);
        stats.bucket.Add(fish.GetComponent<Fish>().value);
        stats.points += fish.GetComponent<Fish>().points;
        Destroy(fish);
        foreach (GameObject go in things)
        {
            go.SetActive(true);
        }
        tempV = new Vector2(-7, -5);
        sr.enabled = true;
        this.enabled = false;
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
        rb.AddForce(new Vector2(Random.Range(0.25f, 1.25f) * -300, Random.Range(0.5f, 1.25f) * 300));
        thrown = true;
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
}
