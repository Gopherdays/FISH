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
    public Fishing hook;
    public PlayerStatsEpic playerStats;
    public bool win;
    
    public float time = 0;
    public TextMeshProUGUI timer;
    public Image foodBar;

    public float bucketTotal;
    public TextMeshProUGUI total;
    public Image bucketBar;
    public bool full;
    float clock;
    bool fishCancel;

    public GameObject creditsObject;
    public string[] credits;

    public GameObject shoppe;
    public GameObject point;
    public GameObject bulk;
    bool bulkTutorial;
    public GameObject feesherCam;

    public GameObject pause;
    public Image transitionImage;
    Color color;

    public Light2D lt;
    public int bulbLvl;
    public int lineLvl;
    public int buckLvl;
    public int reelLvl;

    private void Start()
    {
        bulk.SetActive(false);

        playerStats.Reset();

        color = Color.black;
        StartCoroutine(FadeIn(1));
        
        if (SceneManager.GetActiveScene().name == "Fishing")
        {
            hook = GameObject.Find("Fishing Hook").GetComponent<Fishing>();
            point.SetActive(false);
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
        if (SceneManager.GetActiveScene().name == "Fishing")
        {
            if (clock < 0.1)
                clock += Time.deltaTime;
            else
            {
                UIUpdate();
                clock = 0;
            }

            time += Time.deltaTime;
            playerStats.turtleHunger -= Time.deltaTime * 100 / 180;

            timer.text = "Day " + playerStats.day + " - ";
            if (time > 3600)
            {
                timer.text += Mathf.FloorToInt(time / 3600) + ":";
                if (Mathf.FloorToInt(time) % 3600 < 600) timer.text += "0";
            }
            timer.text += Mathf.FloorToInt(time / 60) % 60 + ":";
            if (Mathf.FloorToInt(time) % 60 < 10) timer.text += "0";
            timer.text += Mathf.FloorToInt(time) % 60;

            if (time >= playerStats.day * 180)
                playerStats.NewDay();

            foodBar.fillAmount = playerStats.turtleHunger / 100;
            if (playerStats.turtleHunger <= 0)
            {
                GoLose();
                playerStats.finalTime = time;
            }
            if (playerStats.bucket.Count >= playerStats.bucketSize)
                full = true;
        }
        else if (SceneManager.GetActiveScene().name == "Game Over")
        {
            time = playerStats.finalTime;
            timer.text = "Day " + playerStats.day + " - ";
            if (time > 3600)
            {
                timer.text += Mathf.FloorToInt(time / 3600) + ":";
                if (Mathf.FloorToInt(time) % 3600 < 600) timer.text += "0";
            }
            timer.text += Mathf.FloorToInt(time / 60) % 60 + ":";
            if (Mathf.FloorToInt(time) % 60 < 10) timer.text += "0";
            timer.text += Mathf.FloorToInt(time) % 60;
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseUnpause();
        }
        if (playerStats.bucket.Count == 0)
            bucketTotal = 0;
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
            bucketTotal += value;
            UIUpdate();
        }
        else
        {
            print("HELP MY BUCKET IS FULLLLLLLLLLLLLLLLL");
        }
    }

    public void UIUpdate()
    {
        bucketBar.fillAmount = (float)playerStats.bucket.Count / playerStats.bucketSize;
        total.text = "$" + bucketTotal;

    }

    public void BuyFood()
    {
        playerStats.money -= playerStats.foodCost;
        playerStats.food++;
    }

    public void UpgradeLight()
    {
        playerStats.money -= playerStats.lightbulbCost;
        if (bulbLvl == 0)
            lt.enabled = true;
        else
            lt.pointLightOuterRadius += 1;
        bulbLvl++;
    }

    public void UpgradeLine()
    {
        playerStats.money -= playerStats.lineSpeedUpgradeCost;
        playerStats.lineSpeedVertical *= 1.5f;
        playerStats.lineSpeedHorizontal *= 1.5f;
        lineLvl++;
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
        if (playerStats.money >= playerStats.bucketSizeCost)
        {
            playerStats.money -= playerStats.bucketSizeCost;
            playerStats.bucketSize += 1;
            buckLvl++;
        }
    }

    public void GoMenu()
    {
        StopAllCoroutines();
        StartCoroutine(FadeOut(1, 0));
    }

    public void GoFish()
    {
        StopAllCoroutines();
        StartCoroutine(FadeOut(1, 1));
    }
    public void GoLose()
    {
        StopAllCoroutines();
        StartCoroutine(FadeOut(1, 2));
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

            GameObject fish = Instantiate(creditsObject, GameObject.Find("Canvas").transform);
            fish.GetComponent<Rigidbody2D>().velocity = Vector2.left * Random.Range(25f, 250f);
            Destroy(fish, 60);
            
            TextMeshProUGUI text = fish.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
            text.text = credits[i];
            i = (i + 1) % credits.Length;

            RectTransform lastFish = fish.GetComponent<RectTransform>();
            pos.y = Random.Range(-260f, -440f);
            lastFish.anchoredPosition = pos;
            lastFish.sizeDelta = new Vector2(text.preferredWidth + 30, text.preferredHeight + 20);

            fish.GetComponent<CapsuleCollider2D>().size = lastFish.sizeDelta;
            fish.GetComponent<Rigidbody2D>().mass = Random.Range(0.1f, 100f);

            yield return new WaitForSeconds(3);
        }
    }

    void SwitchShop()
    {
        shoppe.SetActive(!shoppe.activeSelf);
        feesherCam.SetActive(!feesherCam.activeSelf);
        hook.FreezeUnfreeze();
        if (hook.shopTutorial && shoppe.activeSelf)
            point.SetActive(true);
        else if (hook.shopTutorial && !shoppe.activeSelf)
        {
            hook.shopTutorial = false;
            hook.shop.SetActive(false);
            point.SetActive(false);
        }
        else if (bulkTutorial)
        {
            bulkTutorial = false;
            bulk.SetActive(true);
        }
        else if (bulk.activeSelf)
            bulk.SetActive(false);
    }

    public void B(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (!hook.fishing)
                SwitchShop();
            else if (!fishCancel)
                fishCancel = true;
            else
            {
                fishCancel = false;
            }
        }
    }

    public void Y(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            playerStats.Feed();
        }
    }
    public void A(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (SceneManager.GetActiveScene().name == "Main Menu")
                GoFish();
            else if (SceneManager.GetActiveScene().name == "Game Over")
                GoMenu();
        }
    }
}
