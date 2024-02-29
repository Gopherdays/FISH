using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingLine : MonoBehaviour
{
    LineRenderer line;
    public GameObject origin;
    public GameObject hook;

    Vector3[] positions = new Vector3[3];

    private void Start()
    {
        line = GetComponent<LineRenderer>();
    }

    void Update()
    {
        positions[0] = origin.transform.position;
        positions[1] = hook.transform.position;
        positions[2] = hook.transform.position;
        positions[2].y += 0.45f;
        if (hook.transform.position.y < -0.45f)
            positions[1].y = 0;
        else
            positions[1] = positions[2];
        line.SetPositions(positions);
    }
}
