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

    // fish stats
    float str; // how hard the fish pulls back
    float pauseBase; // the minimum amount of time the fish pauses between pulls
    float pauseExtra; // the maximum amount of random extra time that can be added
    float pullTime; // the amount of time the fish tugs on the line
    float pullExtra; // the maximum amount of random extra time
    float time; // the amount of time you have to reel in the fish before it automatically escapes

    // rod stats
    float rodRes; // how much your rod can bend before it breaks
    float rodStr; // how much force the rod adds

    // tracking variables
    float weight; // how much pressure is on your line
    float length; // how much line is out
    float pull; // the total pull on the line for this frame
    bool pulling;
    bool reeling;
    bool go;

    // timer variables
    float pause; // final pause time
    float timer; // timer variable
    float swim; // how much the fish moves and in what direction
    

    private void OnEnable()
    {
        reel = new InputActions();
        bar.value = 0.75f;
        hook = GameObject.Find("Fishing Hook");
        if (hook.transform.childCount == 1)
        {
            fishStats = hook.transform.GetChild(0).GetComponent<Fish>();
            str = fishStats.str;
            pauseBase = fishStats.pauseBase;
            pauseExtra = fishStats.pauseExtra;
            pullTime = fishStats.pullTime;
            pullExtra = fishStats.pullExtra;
            time = fishStats.time;
        }
        pause = pauseBase + Random.Range(0, pauseExtra);
        reel.Reel.ReelAction.Enable();
        go = true;
    }
    private void FixedUpdate()
    {
        if (go)
        {
            if (pulling)
            {
                Debug.Log("pulling");
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
                Debug.Log("not pulling");
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
    }

    float Calculate()
    {
        swim = 0;
        float strength = 0;
        if (pulling)
            strength = str;
        if (reeling)
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

    public void ReelAction(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            pull += rodStr;
        }
    }
}
