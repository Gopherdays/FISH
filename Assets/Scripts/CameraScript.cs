using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraScript : MonoBehaviour
{
    CinemachineVirtualCamera cam;
    public GameObject hook;

    void Start()
    {
        cam = GetComponent<CinemachineVirtualCamera>();
    }

    void Update()
    {
        if (hook.transform.position.y < -2)
        {
            cam.Follow = hook.transform;
        }
    }
}
