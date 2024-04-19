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
        positions[0] = origin.transform.position + (Vector3)offsetOrigin;
        positions[1] = positions[2] = hook.transform.position;
        positions[2] += (Vector3)offsetHook;
        if (hook.transform.position.y < offsetHook.y)
            positions[1].y = 0;
        else
            positions[1] = positions[2];
        line.SetPositions(positions);
    }
}
