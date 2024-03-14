using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    HookControl hook;
    public bool win;

    public GameObject canvas;
    public float time = 180;
    public TextMeshProUGUI timer;

    public GameObject creditsObject;
    public string[] credits;

    private void Start()
    {
        hook = GameObject.Find("Fishing Hook").GetComponent<HookControl>();
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
            time -= Time.deltaTime;
            timer.text = Mathf.FloorToInt(time / 60) + ":";
            if (Mathf.FloorToInt(time) % 60 < 10) timer.text += "0";
            timer.text += Mathf.FloorToInt(time) % 60;
        }
    }
    public void GoMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void GoShop()
    {
        SceneManager.LoadScene(3);
    }

    public void GoFish()
    {
        SceneManager.LoadScene(1);
    }

    public void GoBoat()
    {
        SceneManager.LoadScene(2);
    }

    public void GoScene(int scene)
    {
        SceneManager.LoadScene(scene);
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
