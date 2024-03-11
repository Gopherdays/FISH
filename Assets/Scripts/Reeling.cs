using System.Collections;
using System.Collections.Generic; // haha I messed with your code    -Nick
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class Reeling : MonoBehaviour
{
    public GameObject canvas;
    public Fish fishStats;
    public Slider bar;
    public InputActions reel;

    public GameObject hook;
    public GameObject player;
    public GameObject fish;

    // fish stats
    float str; // how hard the fish pulls back
    float pauseBase; // the minimum amount of time the fish pauses between pulls
    float pauseExtra; // the maximum amount of random extra time that can be added
    float pullTime; // the amount of time the fish tugs on the line
    float pullExtra; // the maximum amount of random extra time
    float time; // the amount of time you have to reel in the fish before it automatically escapes

    // rod stats
    float rodRes; // how much your rod can bend before it breaks
    float rodStr = 1; // how much force the rod adds

    // tracking variables
    float weight; // how much pressure is on your line
    float length; // how much line is out
    float pull; // the total pull on the line for this frame
    bool pulling;
    float reeling;
    bool go;

    // timer variables
    float pause; // final pause time
    float timer; // timer variable
    float swim; // how much the fish moves and in what direction
    

    private void OnEnable()
    {
        reel = new InputActions();
        bar.value = 75;
        hook = GameObject.Find("Fishing Hook");
        if (hook.transform.childCount == 1)
        {
            fish = hook.transform.GetChild(0).gameObject;
            fishStats = fish.GetComponent<Fish>();
            str = fishStats.str;
            pauseBase = fishStats.pauseBase;
            pauseExtra = fishStats.pauseExtra;
            pullTime = fishStats.pullTime;
            pullExtra = fishStats.pullExtra;
            time = fishStats.time;
        }
        pause = pauseBase + Random.Range(0, pauseExtra);
        reel.Reel.Horizontal.Enable(); ;
        reel.Reel.Vertical.Enable();
        go = true;
    }
    private void FixedUpdate()
    {
        if (go)
        {
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
            reeling -= Time.deltaTime;

        }
    }

    float Calculate()
    {
        swim = 0;
        float strength = 0;
        if (pulling)
            strength = str * 0.33f;
        if (reeling > 0)
        {
            swim = pull + strength;
            if (pulling)
                weight += str;
        }
        else
        {
            swim = strength;
        }
        pull = 0;
        return swim;
    }

    public void Vertical(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            reeling = 0.1f; // Maximum reel potential at 10 inputs/second, reasonable enough I think
            pull += rodStr;
        }
    }

    public void Horizontal(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            reeling = 0.1f; // Maximum reel potential at 10 inputs/second, reasonable enough I think
            pull += rodStr;
        }
    }
}
