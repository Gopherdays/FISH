using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicControl : MonoBehaviour
{
    public AudioSource music;
    public AudioSource shopMusic;
    public GameObject fishing;
    public GameObject shop;

    private void Update()
    {
        if (fishing.activeSelf)
        {
            music.mute = false;
            shopMusic.Stop();
        }
        else if (shop.activeSelf)
        {
            music.mute = true;
            if (!shopMusic.isPlaying)
                shopMusic.Play();
        }
    }
}
