using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;
using TMPro;

public class FishingRod : MonoBehaviour
{
    PlayerInput pi;
    public PlayerStatsEpic playerStats;
    public GameObject fish;
    public GameObject indicator;
    public GameObject catcher;
    public GameObject hook;
    public GameObject[] things;
    public TextMeshProUGUI depthText;
    public FishingLine fl;
    Vector2 tempV;
    List<float> positions = new List<float>();
    int indPos;
    int curFishPos;
    int catcherPos;
    int curCatcherPos;
    bool direction;
    bool skip;
    bool up;
    bool down;
    bool left;
    bool right;
    bool changed;
    public float speed;
    public int turnChance;
    public int skipChance;
    public float rodStr;
    public float distance;
    float timer;
    bool go;
    bool reset;
    private void Start()
    {
        GameObject.Find("Fishing UI").SetActive(false);
        GameObject.Find("Depth Meter").SetActive(false);
        indPos = Random.Range(2, 7);
        indicator.transform.rotation = Quaternion.Euler(0, 0, positions[indPos]);
        curFishPos = indPos;
        this.gameObject.transform.GetChild(2).gameObject.SetActive(false);
        this.gameObject.transform.GetChild(3).gameObject.SetActive(false);
        this.enabled = false;
        tempV = new Vector2(-7, -5);
    }

    private void OnEnable()
    {
        pi = GetComponent<PlayerInput>();
        GameObject.Find("Virtual Camera").GetComponent<CinemachineVirtualCamera>().Follow = GameObject.Find("Player").transform;
        for (int i = 0; i < 8; i++)
        {
            positions.Add(i * 45);
        }
        this.gameObject.transform.GetChild(2).gameObject.SetActive(true);
        this.gameObject.transform.GetChild(3).gameObject.SetActive(true);
        direction = true;
        skip = false;
        skipChance = 10;
        turnChance = 30;
        indPos = Random.Range(2, 7);
        catcherPos = 1;
        curCatcherPos = 1;
        curFishPos = indPos;
        rodStr = 3;

        go = true;
    }
    private void Update()
    {
        if (go)
        {
            if (changed)
            {
                changed = false;
                switch (true)
                {
                    case true when (up && !left && !right):
                        catcherPos = 0;
                        break;
                    case true when (up && left):
                        catcherPos = 1;
                        break;
                    case true when (left && !up && !down):
                        catcherPos = 2;
                        break;
                    case true when (down && left):
                        catcherPos = 3;
                        break;
                    case true when (down && !left && !right):
                        catcherPos = 4;
                        break;
                    case true when (down && right):
                        catcherPos = 5;
                        break;
                    case true when (right && !up && !down):
                        catcherPos = 6;
                        break;
                    case true when (up && right):
                        catcherPos = 7;
                        break;
                    default:
                        break;
                }
            }
        }
    }
    private void FixedUpdate()
    {
        if (go)
        {
            if (timer < speed)
                timer += Time.deltaTime;
            else
            {
                indPos = NewPos(indPos);
                // print(indPos);
                timer = 0;
            }

            if (indPos != curFishPos)
            {
                indicator.transform.rotation = Quaternion.Euler(0, 0, positions[indPos]);
                curFishPos = indPos;
            }

            if (catcherPos != curCatcherPos)
            {
                catcher.transform.rotation = Quaternion.Euler(0, 0, positions[catcherPos]);
                curCatcherPos = catcherPos;
            }

            if (catcherPos == indPos)
                distance -= rodStr * Time.deltaTime;

            else if (NewInt(catcherPos - 1) == indPos || NewInt(catcherPos + 1) == indPos)
                distance -= rodStr * Time.deltaTime * 0.5f;

            else
                distance += speed * Time.deltaTime * 3;

            depthText.text = (int)distance + "m";
            if (distance <= 0)
            {
                go = false;
                reset = true;
            }
        }
        else if (reset)
        {
            if (fish.transform.position.y <= -1.5f)
            {
                tempV.y += Time.deltaTime * 1.5f;
                fish.transform.position = tempV;
            }
            else
            {
                print("Yay! You caught: " + fish.name.Remove(fish.name.Length-7));
                GameObject.Find("Fishing UI").SetActive(false);
                GameObject.Find("Depth Meter").SetActive(false);
                GameObject.Find("Virtual Camera").GetComponent<CameraScript>().cast = false;
                fl.hook = hook;
                HookControl hooky = hook.GetComponent<HookControl>();
                hooky.ThrowHook();
                hooky.enabled = true;
                hook.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
                hook.SetActive(true);
                this.enabled = false;
                playerStats.bucket.Add(fish);
                Destroy(fish);
                foreach (GameObject go in things)
                {
                    go.SetActive(true);
                }
            }
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

    public void Up(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            up = true;
            changed = true;
        }
        if (context.canceled)
        {
            up = false;
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