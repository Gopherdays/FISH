using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish : MonoBehaviour
{
    public float str; // how hard the fish pulls back
    public float pass; // the passive pull (if you don't reel, the fish will drift out)
    public float pauseBase; // the minimum amount of time the fish pauses between pulls
    public float pauseExtra; // the maximum amount of random extra time that can be added
    public float time; // the amount of time you have to reel in the fish before it automatically escapes

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
