using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Splash : MonoBehaviour
{
    bool above;

    private void Awake()
    {
        if (transform.position.y > 0)
            above = true;
        else
            above = false;
    }
    private void FixedUpdate()
    {
        if (transform.position.y < 0 && above)
        {
            Effect();
        }
    }
    void Effect()
    {

    }
}
