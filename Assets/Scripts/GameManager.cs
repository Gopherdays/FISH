using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    HookControl hook;
    Reeling reeler;
    
    public bool win;

    private void Start()
    {
        hook = GameObject.Find("Fishing Hook").GetComponent<HookControl>();
        reeler = GameObject.Find("Virtual Camera").GetComponent<Reeling>();
        reeler.enabled = false;
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
            GameObject canvas = GameObject.Find("Virtual Camera").transform.GetChild(0).gameObject;
            canvas.SetActive(true);
        }
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
