using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Rendering.UI;

public class GameManager : MonoBehaviour
{
    HookControl hook;
    public bool win;

    public GameObject canvas;
    public float time = 180;
    public TextMeshProUGUI timer;
    public TextMeshProUGUI HHHURRYUPP;

    public GameObject creditsObject;
    public string[] credits;

    public GameObject pause;
    public Image transitionImage;
    public bool fade = true;
    Color color;

    private void Start()
    {
        if (fade)
        {
            color = Color.black;
            StartCoroutine(FadeInOut(-1));
        }
        if (SceneManager.GetActiveScene().name == "Fishing")
        {
            hook = GameObject.Find("Fishing Hook").GetComponent<HookControl>();
            HHHURRYUPP.alpha = 0;
        }
        pause.SetActive(false);
    }
    
    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "Main Menu" && time > 0)
        {
            StartCoroutine(CreditsSpawning());
            time = -69;
        }
        if (win)
        {
            GoBoat();
            win = false;
        }
        if (SceneManager.GetActiveScene().name == "Fishing" && hook.fishing)
        {
            if (time <= -60) // If you can't reel in one fish in an entire minute that's a skill issue
            {
                GoBoat();
                hook.fishing = false;
            }
            else
                time -= Time.deltaTime;

            // We need to check if the player is actively reeling in a fish. If they aren't and the time is below 0, then go to boat. I need an isReeling variable

            if (time > 0)
            {
                timer.text = Mathf.FloorToInt(time / 60) + ":";
                if (Mathf.FloorToInt(time) % 60 < 10) timer.text += "0";
                timer.text += Mathf.FloorToInt(time) % 60;
            }
            else timer.text = "0:00";
            HHHURRYUPP.text = Mathf.FloorToInt((time + 61) / 60) + ":";
            if (Mathf.FloorToInt(time+61) % 60 < 10) HHHURRYUPP.text += "0";
            HHHURRYUPP.text += Mathf.FloorToInt(time+61) % 60;
            HHHURRYUPP.alpha = (-time - 30) / 300;
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseUnpause();
        }
    }

    public IEnumerator FadeInOut(float speed, int scene = -1)
    {
        ForceUnpause();
        while (color.a > -0.1f && color.a < 1.1f)
        {
            color.a += Time.deltaTime * (1 / speed);
            transitionImage.color = color;
            yield return new WaitForFixedUpdate();
        }
        if (scene >= 0)
        {
            SceneManager.LoadScene(scene);
        }
        yield break;
    }

    public void PauseUnpause()
    {
        if (pause.activeSelf)
        {
            pause.SetActive(false);
            Time.timeScale = 1;
        }
        else
        {
            pause.SetActive(true);
            Time.timeScale = 0;
        }
    }
    public void ForceUnpause()
    {
        pause.SetActive(false);
        Time.timeScale = 1;
    }

    // Voids that start coroutines used to allow activation through buttons because that seems to happen a lot
    public void GoMenu()
    {
        StopAllCoroutines();
        StartCoroutine(FadeInOut(1, 0));
    }

    public void GoShop()
    {
        StopAllCoroutines();
        StartCoroutine(FadeInOut(1, 3));
    }

    public void GoFish()
    {
        StopAllCoroutines();
        StartCoroutine(FadeInOut(1, 1));
    }

    public void GoBoat()
    {
        StopAllCoroutines();
        StartCoroutine(FadeInOut(1, 2));
    }

    public void GoScene(int scene)
    {
        StopAllCoroutines();
        StartCoroutine(FadeInOut(1, scene));
    }

    IEnumerator CreditsSpawning()
    {
        Vector2 pos = new(1200, -270);
        int i = 0;
        while (true)
        {
            // Make a credits object and send it to the left towards its DOOM
            GameObject fish = Instantiate(creditsObject, GameObject.Find("Canvas").transform);
            fish.GetComponent<Rigidbody2D>().velocity = Vector2.left * Random.Range(25f, 250f);
            Destroy(fish, 60);
            // Cycle the credits text
            TextMeshProUGUI text = fish.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
            text.text = credits[i];
            i = (i + 1) % credits.Length;
            // Random y positions of the rectTransforms, and resize the objects to fit the given text
            RectTransform lastFish = fish.GetComponent<RectTransform>();
            pos.y = Random.Range(-260f, -440f);
            lastFish.anchoredPosition = pos;
            lastFish.sizeDelta = new(text.preferredWidth + 30, text.preferredHeight + 20);
            // Collisions for fun
            fish.GetComponent<CapsuleCollider2D>().size = lastFish.sizeDelta;
            fish.GetComponent<Rigidbody2D>().mass = Random.Range(0.1f, 100f);
            // Make a new one after three seconds
            yield return new WaitForSeconds(3);
        }
    }
}
