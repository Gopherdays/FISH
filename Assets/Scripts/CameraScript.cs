using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Rendering.Universal;

public class CameraScript : MonoBehaviour
{
    //Camera component
    CinemachineVirtualCamera cam;

    //Camera shake noise
    //CinemachineBasicMultiChannelPerlin noise;

    //References
    public GameManager gm;
    public GameObject hook;
    public GameObject sun;
    public GameObject moon;
    public GameObject mountains;
    public GameObject cloud;

    //Skybox
    public Gradient skyGradient;
    Light2D test;
    bool cast;

    void Start()
    {
        //Get stuff
        cam = GetComponent<CinemachineVirtualCamera>();
        //noise = cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        test = GameObject.Find("Global Light 2D").GetComponent<Light2D>();

        //Spawn clouds
        for (int i = 0; i < Random.Range(4, 11); i++)
        {
            Instantiate(cloud, Vector2.right * Random.Range(-14, 14f) + Vector2.up * Random.Range(1,7.5f), Quaternion.identity, transform);
        }
    }

    void Update()
    {
        //Sky light intensity based off of the green channel of the sky gradient
        test.intensity = skyGradient.Evaluate((gm.time % 180) / 180).g * 255 / 203;

        /// Sun and moon position/rotation code
        /// X and Y position for the sun can actually be made really easily with MATH!!!! TRIGONOMETRY!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        /// Cos and Sin measure the X and Y of an angle on the unit circle...
        /// ...so you can multiply the result by the width and height of the orbit you want
        /// I knew all that math class would pay off some day
        sun.transform.SetLocalPositionAndRotation(new Vector3(
            Mathf.Cos((gm.time * 2) * Mathf.Deg2Rad) * 8,
            (Mathf.Sin((gm.time * 2) * Mathf.Deg2Rad) * 5) - 1 - (cam.transform.position.y / 5), 10),
            Quaternion.Euler(0, 0, gm.time * 2));
        // Of course, the moon is directly opposite of the sun because we're exactly like Minecraft, as one of my non-gamer classmates said that one time
        moon.transform.SetLocalPositionAndRotation(new Vector3(
            Mathf.Cos((gm.time * 2 + 180) * Mathf.Deg2Rad) * 8,
            (Mathf.Sin((gm.time * 2 + 180) * Mathf.Deg2Rad) * 5) - 1 - (cam.transform.position.y / 5), 10),
            Quaternion.Euler(0, 0, gm.time * 2));

        //Mountains follow the camera as well, but only a little to create parallax effect
        mountains.transform.SetLocalPositionAndRotation(new Vector3(transform.position.x / -4, -6.835f - (cam.transform.position.y / 4), 10), Quaternion.identity);

        //Follow the hook if it goes below the water
        if (hook.transform.position.y < 0 && !cast)
        {
            cast = true;
            cam.Follow = hook.transform;
            //cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = 0.7f;
        }
        /*Slow down camera shake over time
        if (noise.m_FrequencyGain > 1)
            noise.m_FrequencyGain *= 0.95f;
        else
            noise.m_FrequencyGain = 1;
        */
        //Set sky color to the gradient
        Camera.main.backgroundColor = skyGradient.Evaluate((gm.time % 180) / 180);

        //If there's less than 10 clouds, make new clouds
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
        //noise.m_FrequencyGain *= intensity;
    }
}
