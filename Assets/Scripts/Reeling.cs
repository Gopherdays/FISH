using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reeling : MonoBehaviour
{
    GameObject fish;


    float str; // how hard the fish pulls back
    float pass; // the passive pull (if you don't reel, the fish will drift out)
    float pauseBase; // the minimum amount of time the fish pauses between pulls
    float pauseExtra; // the maximum amount of random extra time that can be added
    float time; // the amount of time you have to reel in the fish before it automatically escapes

    float maxLength; // the maximum length you can let your line let out before the fish escapes
    float rodRes; // how much your rod can bend before it breaks
    float rodStr; // how much force the rod adds

    float weight; // how much pressure is on your line
    float length; // how much line is out

    float pause; // final pause time
    float timer; // timer variable
    float swim; // how much the fish moves and in what direction
    bool pulling;
    bool reeling;

    private void Start()
    {
        Fish feesh = fish.GetComponent<Fish>();
        feesh.str = str;
        feesh.pass = pass;
        feesh.pauseBase = pauseBase;
        feesh.pauseExtra = pauseExtra;
        feesh.time = time;
    }
    private void FixedUpdate()
    {
        if (pulling)
        {

        }
        else
        {

        }
        pause = pauseBase + Random.Range(0, pauseExtra);
        if (timer < pause)
            timer += Time.deltaTime;
        else
        {
            pulling = true;
        }
    }
    float Calculate()
    {
        float strength;
        if (pulling)
        {
            strength = str;
        }
        return swim;
    }
}
