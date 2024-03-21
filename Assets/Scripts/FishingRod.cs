using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;

public class FishingRod : MonoBehaviour
{
    PlayerInput pi;
    public GameObject fish;
    public GameObject catcher;
    List<float> positions = new List<float>();
    int fishPos;
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

    private void Start()
    {
        pi = GetComponent<PlayerInput>();
        GameObject.Find("Virtual Camera").GetComponent<CinemachineVirtualCamera>().Follow = GameObject.Find("Player").transform;
        for (int i = 0; i < 8; i++)
        {
            positions.Add(i * 45);
            print(i + " iteration");
        }
        direction = true;
        skip = false;
        skipChance = 10;
        turnChance = 30;
        fishPos = Random.Range(2, 7);
        catcherPos = 1;
        curCatcherPos = 1;
        curFishPos = fishPos;
        rodStr = 3;
        distance = 50;
    }
    private void Update()
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
                    print("There is no change, but we thought");
                    break;
            }
        }
    }
    private void FixedUpdate()
    {
        if (timer < speed)
            timer += Time.deltaTime;
        else
        {
            fishPos = newPos(fishPos);
           // print(fishPos);
            timer = 0;
        }

        if (fishPos != curFishPos)
        {
            fish.transform.rotation = Quaternion.Euler(0, 0, positions[fishPos]);
            curFishPos = fishPos;
        }

        if (catcherPos != curCatcherPos)
        {
            catcher.transform.rotation = Quaternion.Euler(0, 0, positions[catcherPos]);
            curCatcherPos = catcherPos;
        }

        if (catcherPos == fishPos)
        {
            distance -= rodStr * Time.deltaTime;
            print((int)distance);
        }
        else if (newInt(catcherPos - 1) == fishPos || newInt(catcherPos + 1) == fishPos)
        {
            distance -= rodStr * Time.deltaTime * 0.5f;
            print((int)distance);
        }
        else
        {
            distance += speed * Time.deltaTime * 3;
            print((int)distance);
        }
    }
    int newPos(int position)
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
        temp = newInt(temp);
        skip = false;
        return temp;
    }
    int newInt(int temp)
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