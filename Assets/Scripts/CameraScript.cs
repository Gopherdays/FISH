using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraScript : MonoBehaviour
{
    CinemachineVirtualCamera cam;
    CinemachineBasicMultiChannelPerlin noise;

    public GameManager gm;
    public GameObject hook;
    public GameObject sun;
    public GameObject moon;
    public Gradient skyGradient;
    public bool cast;

    void Start()
    {
        cam = GetComponent<CinemachineVirtualCamera>();
        noise = cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    void Update()
    {
        sun.transform.SetLocalPositionAndRotation(new Vector3(Mathf.Cos((gm.time * 1.5f - 120) * Mathf.Deg2Rad) * 8, (Mathf.Sin((gm.time * 1.5f - 120) * Mathf.Deg2Rad) * 5) - 1 - (cam.transform.position.y / 4), 10), Quaternion.identity);
        moon.transform.SetLocalPositionAndRotation(new Vector3(Mathf.Cos((gm.time * 1.5f + 80) * Mathf.Deg2Rad) * 8, (Mathf.Sin((gm.time * 1.5f + 80) * Mathf.Deg2Rad) * 5) - 1 - (cam.transform.position.y / 4), 10), Quaternion.identity);
        if (hook.transform.position.y < 0 && !cast)
        {
            cast = true;
            cam.Follow = hook.transform;
            cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = 0.7f;
        }
        if (noise.m_FrequencyGain > 1)
            noise.m_FrequencyGain *= 0.95f;
        else
            noise.m_FrequencyGain = 1;
        Camera.main.backgroundColor = skyGradient.Evaluate((180 - gm.time) / 180);
        if (gm.time < 0)
            sun.GetComponent<SpriteRenderer>().enabled = false;
    }

    public void Shake(float intensity)
    {
        noise.m_FrequencyGain *= intensity;
    }
}
