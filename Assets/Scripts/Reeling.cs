using System.Collections;
using System.Collections.Generic; // haha I messed with your code    -Nick
using UnityEngine;
using UnityEngine.UI;

public class Reeling : MonoBehaviour
{
    public GameObject fishStats;
    public Slider bar;

    // fish stats
    float str; // how hard the fish pulls back
    float pauseBase; // the minimum amount of time the fish pauses between pulls
    float pauseExtra; // the maximum amount of random extra time that can be added
    float pullTime; // the amount of time the fish tugs on the line
    float pullExtra; // the maximum amount of random extra time
    float time; // the amount of time you have to reel in the fish before it automatically escapes

    // rod stats
    float maxLength; // the maximum length you can let your line let out before the fish escapes
    float rodRes; // how much your rod can bend before it breaks
    float rodStr; // how much force the rod adds

    // tracking variables
    float weight; // how much pressure is on your line
    float length; // how much line is out
    bool pulling;
    bool reeling;

    // timer variables
    float pause; // final pause time
    float timer; // timer variable
    float swim; // how much the fish moves and in what direction

    private void Start()
    {
        bar.value = 0.75f;
        //Fish feesh = fish.GetComponent<Fish>();
        //feesh.str = str;
        //feesh.pauseBase = pauseBase;
        //feesh.pauseExtra = pauseExtra;
        //feesh.pullTime = pullTime;
        //feesh.pullExtra = pullExtra;
        //feesh.time = time;
        maxLength = 1;
        pause = pauseBase + Random.Range(0, pauseExtra);
        length = maxLength * 0.75f;
    }
    private void Update()
    {
        
    }
    private void FixedUpdate()
    {
        if (bar.value == 0)
        if (pulling)
        {
            if (timer < pullTime)
                timer += Time.deltaTime;
            else
            {
                timer = 0;
                pause = pauseBase + Random.Range(0, pauseExtra);
                pulling = false;
            }
        }
        else
        {
            if (timer < pause)
                timer += Time.deltaTime;
            else
            {
                timer = 0;
                pulling = true;
            }
        }
        bar.value += Calculate();
    }
    float Calculate()
    {
        float strength = 0;
        if (pulling)
            strength = str;
        if (reeling)
        {
            swim = rodStr + strength;
            if (pulling)
                weight += str;
        }
        else
        {
            swim = strength;
        }
        return swim * 3;
    }
}
