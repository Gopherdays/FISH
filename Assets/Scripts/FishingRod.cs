using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingRod : MonoBehaviour
{
    GameObject fish;
    GameObject catcher;
    List<float> positions;
    int fishPos;
    int curFishPos;
    int catcherPos;
    int curCatcherPos;
    bool direction;
    bool skip;
    public float speed;
    public int randomness;
    float timer;
    private void OnEnable()
    {
        randomness = 10;
        fishPos = Random.Range(2, 7);
        catcherPos = 1;
        curCatcherPos = 1;
        curFishPos = fishPos;
    }
    private void FixedUpdate()
    {
        if (timer < speed)
            timer += Time.deltaTime;
        else
        {
            newPos(fishPos);
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
    }
    int newPos(int position)
    {
        if (Random.Range(1, 101) >= randomness)
            direction = !direction;

        if (Random.Range(1, 101) >= randomness)
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
        skip = false;
        return temp;
    }
}