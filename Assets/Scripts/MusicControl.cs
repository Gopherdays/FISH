using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicControl : MonoBehaviour
{
    public AudioSource music;
    public AudioSource intenseMoment;
    public AudioSource waiting;
    Fishing fishing;

    private void Start()
    {
        fishing = GameObject.Find("Fishing Hook").GetComponent<Fishing>();
    }

    private void Update()
    {
        if (fishing.thrown && !music.isPlaying)
        {
            music.Play();
            waiting.Stop();
            intenseMoment.Play();
            intenseMoment.mute = true;
        }
        intenseMoment.mute = !(Input.GetKey(KeyCode.P)); // Replace input with reeling variable in hookControl
    }
}
