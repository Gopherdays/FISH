using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Rendering.Universal;

public class CameraScript : MonoBehaviour
{
    CinemachineVirtualCamera cam;
    CinemachineBasicMultiChannelPerlin noise;

    public GameManager gm;
    public GameObject hook;
    public GameObject sun;
    public GameObject moon;
    public GameObject mountains;
    public GameObject cloud;
    public Gradient skyGradient;
    Light2D test;
    public bool cast;

    void Start()
    {
        cam = GetComponent<CinemachineVirtualCamera>();
        noise = cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        test = GameObject.Find("Global Light 2D").GetComponent<Light2D>();
        for (int i = 0; i < Random.Range(4, 11); i++)
        {
            Instantiate(cloud, Vector2.right * Random.Range(-14, 14f) + Vector2.up * Random.Range(1,7.5f), Quaternion.identity, transform);
        }
    }

    void Update()
    {
        test.intensity = skyGradient.Evaluate((gm.time % 180) / 180).g * 255 / 203;
        sun.transform.SetLocalPositionAndRotation(new Vector3(Mathf.Cos((gm.time * 2) * Mathf.Deg2Rad) * 8, (Mathf.Sin((gm.time * 2) * Mathf.Deg2Rad) * 5) - 1 - (cam.transform.position.y / 5), 10), Quaternion.Euler(0,0,gm.time * 2));
        moon.transform.SetLocalPositionAndRotation(new Vector3(Mathf.Cos((gm.time * 2 + 180) * Mathf.Deg2Rad) * 8, (Mathf.Sin((gm.time * 2 + 180) * Mathf.Deg2Rad) * 5) - 1 - (cam.transform.position.y / 5), 10), Quaternion.Euler(0, 0, gm.time * 2));
        mountains.transform.SetLocalPositionAndRotation(new Vector3(transform.position.x / -4, -6.835f - (cam.transform.position.y / 4), 10), Quaternion.identity);
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
        Camera.main.backgroundColor = skyGradient.Evaluate((gm.time % 180) / 180);

        if (transform.childCount < 11)
        {
            for (int i = 0; i < Random.Range(4,13); i++)
            {
                Instantiate(cloud, Vector2.right * Random.Range(10, 17f) + Vector2.up * Random.Range(1, 7.5f), Quaternion.identity, transform);
            }
        }
    }

    public void Shake(float intensity)
    {
        noise.m_FrequencyGain *= intensity;
    }
}
