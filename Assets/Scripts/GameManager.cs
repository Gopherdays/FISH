using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

public class GameManager : MonoBehaviour
{
    Fishing hook;
    public PlayerStatsEpic playerStats;
    public bool win;
    
    public float time = 0;
    public TextMeshProUGUI timer;
    public Image foodBar;

    public float bucketTotal;
    public TextMeshProUGUI total;
    public Image bucketBar;
    public bool full;

    public GameObject creditsObject;
    public string[] credits;

    public GameObject shoppe;
    public GameObject feesherCam;

    public GameObject pause;
    public Image transitionImage;
    Color color;

    Light2D light;
    int bulbLvl;
    int lineLvl;
    int buckLvl;
    int reelLvl;

    private void Start()
    {
        playerStats.Reset();

        color = Color.black;
        StartCoroutine(FadeIn(1));
        
        if (SceneManager.GetActiveScene().name == "Fishing")
        {
            hook = GameObject.Find("Fishing Hook").GetComponent<Fishing>();
        }
        pause.SetActive(false);
        if (SceneManager.GetActiveScene().name == "Main Menu")
        {
            StartCoroutine(CreditsSpawning());
            time = -69;
            playerStats.Reset();
        }
    }
    
    private void Update()
    {
        if (win)
        {
            GoBoat();
            win = false;
        }
        if (SceneManager.GetActiveScene().name == "Fishing")
        {
            time += Time.deltaTime;
            playerStats.turtleHunger -= Time.deltaTime * 100 / 180;

            timer.text = "Day " + playerStats.day + " - ";
            if (time > 3600)
            {
                timer.text += Mathf.FloorToInt(time / 3600) + ":";
                if (Mathf.FloorToInt(time) % 3600 < 600) timer.text += "0";
            }
            timer.text += Mathf.FloorToInt(time / 60) + ":";
            if (Mathf.FloorToInt(time) % 60 < 10) timer.text += "0";
            timer.text += Mathf.FloorToInt(time) % 60;

            if (time >= playerStats.day * 180)
                playerStats.NewDay();

            foodBar.fillAmount = playerStats.turtleHunger / 100;
            if (playerStats.turtleHunger <= 0)
            {
                GoLose();
            }
            if (playerStats.bucket.Count >= playerStats.bucketSize)
                full = true;
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseUnpause();
        }
    }

    public IEnumerator FadeIn(float speed, int scene = -1)
    {
        ForceUnpause();
        while (color.a > -0.1f)
        {
            color.a -= Time.deltaTime * (1 / speed);
            transitionImage.color = color;
            yield return new WaitForFixedUpdate();
        }
        yield break;
    }

    public IEnumerator FadeOut(float speed, int scene)
    {
        ForceUnpause();
        while (color.a < 1.1f)
        {
            color.a += Time.deltaTime * (1 / speed);
            transitionImage.color = color;
            yield return new WaitForFixedUpdate();
        }
        SceneManager.LoadScene(scene);
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
    public void AddFish(int value)
    {
        if (playerStats.bucket.Count < playerStats.bucketSize)
        {
            playerStats.BucketAdd(value);
            bucketBar.fillAmount = (float)playerStats.bucket.Count / playerStats.bucketSize;
            bucketTotal += value;
            total.text = "$" + bucketTotal;
        }
        else
        {
            print("HELP MY BUCKET IS FULLLLLLLLLLLLLLLLL");
        }
    }
    public void UpgradeLight()
    {
        if (playerStats.money >= playerStats.lightbulbCost)
        {
            playerStats.money -= playerStats.lightbulbCost;
            if (bulbLvl == 0)
                light.enabled = true;
            else
                light.pointLightOuterRadius += 1;
            bulbLvl++;
        }
    }
    public void UpgradeLine()
    {
        if (playerStats.money >= playerStats.lineSpeedUpgradeCost)
        {
            playerStats.money -= playerStats.lineSpeedUpgradeCost;
            playerStats.lineSpeedVertical *= 1.5f;
            playerStats.lineSpeedHorizontal *= 1.5f;
            lineLvl++;
        }
    }
    public void UpgradeReel()
    {
        if (playerStats.money >= playerStats.strengthMultUpgradeCost)
        {
            playerStats.money -= playerStats.strengthMultUpgradeCost;
            playerStats.strengthMult += 2;
            reelLvl++;
        }
    }
    public void UpgradeBucket()
    {

    }
    // Voids that start coroutines used to allow activation through buttons because that seems to happen a lot
    public void GoMenu()
    {
        StopAllCoroutines();
        StartCoroutine(FadeOut(1, 0));
    }

    public void GoShop()
    {
        StopAllCoroutines();
        StartCoroutine(FadeOut(1, 3));
    }

    public void GoFish()
    {
        StopAllCoroutines();
        StartCoroutine(FadeOut(1, 1));
    }

    public void GoBoat()
    {
        StopAllCoroutines();
        StartCoroutine(FadeOut(1, 2));
    }

    public void GoLose()
    {
        StopAllCoroutines();
        StartCoroutine(FadeOut(1, 4));
    }

    public void GoScene(int scene)
    {
        StopAllCoroutines();
        StartCoroutine(FadeOut(1, scene));
    }

    IEnumerator CreditsSpawning()
    {
        Vector2 pos = new Vector2(1200, -270);
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
            lastFish.sizeDelta = new Vector2(text.preferredWidth + 30, text.preferredHeight + 20);
            // Collisions for fun
            fish.GetComponent<CapsuleCollider2D>().size = lastFish.sizeDelta;
            fish.GetComponent<Rigidbody2D>().mass = Random.Range(0.1f, 100f);
            // Make a new one after three seconds
            yield return new WaitForSeconds(3);
        }
    }

    void SwitchShop()
    {
        shoppe.SetActive(!shoppe.activeSelf);
        feesherCam.SetActive(!feesherCam.activeSelf);
    }

    public void B(InputAction.CallbackContext context)
    {
        if (context.started) // on button press
        {
            SwitchShop();
        }
        if (context.performed) // on button hold
        {

        }
        if (context.canceled) // on button 
        {

        }
    }
    public void Y(InputAction.CallbackContext context)
    {
        if (context.started) // on button press
        {
            playerStats.Feed();
        }
    }
}
