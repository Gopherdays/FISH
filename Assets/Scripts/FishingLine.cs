using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingLine : MonoBehaviour
{
    LineRenderer line;
    public GameObject origin;
    public Vector2 offsetOrigin;
    public GameObject hook;
    public Vector2 offsetHook;

    Vector3[] positions = new Vector3[3];

    private void Start()
    {
        line = GetComponent<LineRenderer>();
    }

    void Update()
    {
        //First position goes from the player's fishing rod
        positions[0] = origin.transform.position + (Vector3)offsetOrigin;

        //Second and third position are set to the hook's loop
        positions[1] = positions[2] = hook.transform.position + (Vector3)offsetHook;

        //If the hook is underwater, the second position sits at the surface of the water
        if (hook.transform.position.y < offsetHook.y)
            positions[1].y = 0;
        else
            positions[1] = positions[2];

        //Put the positions into the line renderer
        line.SetPositions(positions);
    }
}
