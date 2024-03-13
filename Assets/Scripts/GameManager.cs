using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    HookControl hook;
    Reeling reeler;
    public bool win;

    public GameObject canvas;
    public float time = 180;
    public TextMeshProUGUI timer;

    public GameObject creditsObject;
    public string[] credits;

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
        if (SceneManager.GetActiveScene().name == "Main Menu")
        {
            StartCoroutine(CreditsSpawning());
        }
        if (win)
        {
            GoBoat();
            win = false;
        }
        if (SceneManager.GetActiveScene().name == "Fishing" && hook.fishing)
        {
            reeler.enabled = true;
            canvas.SetActive(true);
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

    IEnumerator CreditsSpawning()
    {
        RectTransform lastFish = null;
        Vector2 pos = new(1100, -270);
        int i = 0;
        while (true)
        {
            GameObject fish = Instantiate(creditsObject);
            fish.GetComponent<Rigidbody2D>().velocity = Vector2.left * Random.Range(2f, 5f);
            TextMeshProUGUI text = fish.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
            text.text = credits[i];
            i++;
            if (lastFish != null || pos.y == -430)
                pos.y = -270;
            else
                pos.y = -430;
            lastFish = fish.GetComponent<RectTransform>();
            lastFish.anchoredPosition = pos;
            lastFish.sizeDelta = new(text.preferredWidth + 30, text.preferredHeight + 20);
            yield return new WaitUntil(() => lastFish.anchoredPosition.x < 550);
        }
    }
}
