using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicControl : MonoBehaviour
{
    public AudioSource music;
    public AudioSource shopMusic;
    public AudioSource pauseMusic;
    public GameObject fishing;
    public GameObject shop;
    public GameManager gm;

    private void Update()
    {
        if (fishing.activeSelf && !gm.pause.activeSelf)
        {
            music.mute = false;
            shopMusic.Stop();
        }
        else if (shop.activeSelf && !gm.pause.activeSelf)
        {
            music.mute = true;
            if (!shopMusic.isPlaying)
                shopMusic.Play();
        }
    }
}
