using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    HookControl hook;
    Reeling reeler;
    public GameObject canvas;
    public float time = 180;
    public TextMeshProUGUI timer;
    public bool win;

    private void Start()
    {
        hook = GameObject.Find("Fishing Hook").GetComponent<HookControl>();
        reeler = GameObject.Find("Virtual Camera").GetComponent<Reeling>();
        reeler.enabled = false;
        //canvas = GameObject.Find("Virtual Camera").transform.GetChild(0).gameObject;
        canvas.SetActive(false);
    }
    private void Update()
    {
        if (win)
        {
            GoBoat();
            win = false;
        }
        if (/*SceneManager.GetActiveScene().buildIndex == #Insert Build Number# && */hook.fishing)
        {
            reeler.enabled = true;
            canvas.SetActive(true);
        }
        time -= Time.deltaTime;
        timer.text = Mathf.FloorToInt(time / 60) + ":" + Mathf.CeilToInt((time - Mathf.Floor(time / 60)) % 59);
    }
    public void GoMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void GoShop()
    {
        SceneManager.LoadScene(0); // put in shop scene number
    }

    public void GoFish()
    {
        SceneManager.LoadScene(0); // put in fishing scene number
    }

    public void GoBoat()
    {
        SceneManager.LoadScene(0); // put in overworld scene number
    }

    public void GoScene(int scene)
    {
        SceneManager.LoadScene(scene);
    }
}
