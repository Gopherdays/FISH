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
    bool part1;
    bool part2;
    bool part3;
    float resetTimer;

    [SerializeField] GameObject controls;
    [SerializeField] TextMeshProUGUI tutorialText;
    static bool tutorialOn;

    public Fishing hook;
    public PlayerStatsEpic playerStats;
    public WhatTheSavema whatTheSavema;
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
        tutorialOn = true;
        if (controls != null)
            controls.SetActive(false);
        color = Color.black;
        StartCoroutine(FadeIn(1));
        
        if (SceneManager.GetActiveScene().name == "Fishing")
        {
            bulk.SetActive(false);
            hook = GameObject.Find("Fishing Hook").GetComponent<Fishing>();
            point.SetActive(false);
        }
        pause.SetActive(false);
        if (SceneManager.GetActiveScene().name == "Main Menu")
        {
            StartCoroutine(CreditsSpawning());
            time = -69;
            playerStats.Reset();
            if (whatTheSavema != null)
                whatTheSavema.Load();
        }
        if (SceneManager.GetActiveScene().name == "Game Over")
        {
            // do the high score thing
            whatTheSavema.Load();
            whatTheSavema.highScores.scores.Add(new("BBBB", playerStats.finalTime, playerStats.points, playerStats.discoveredFish.Count));
            whatTheSavema.Save();
            whatTheSavema.ArrangeScores();
        }
        bulkTutorial = true;
    }
    private void Awake()
    {
        if (!tutorialOn && SceneManager.GetActiveScene().name == "Fishing")
        {
            hook.tutorial = false;
            hook.foodTutorial = false;
            hook.shopTutorial = false;
        }
    }

    private void Update()
    {
        if (part1 && part2 && part3)
        {
            if (resetTimer < 5)
                resetTimer += Time.deltaTime;
            else
            {
                resetTimer = 0;
                GoMenu();
            }
        }
        else
        {
            if (resetTimer != 0)
                resetTimer = 0;
        }
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

            timer.text = TimeFormat(time);

            

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
            /*time = playerStats.finalTime;
            timer.text = "Day " + playerStats.day + " - ";
            if (time > 3600)
            {
                timer.text += Mathf.FloorToInt(time / 3600) + ":";
                if (Mathf.FloorToInt(time) % 3600 < 600) timer.text += "0";
            }
            timer.text += Mathf.FloorToInt(time / 60) % 60 + ":";
            if (Mathf.FloorToInt(time) % 60 < 10) timer.text += "0";
            timer.text += Mathf.FloorToInt(time) % 60;*/
            whatTheSavema.ArrangeScores();
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

    public void AddFish(Fish fish, float mult)
    {
        if (playerStats.bucket.Count < playerStats.bucketSize)
        {
            playerStats.BucketAdd(Mathf.RoundToInt(fish.value * mult));
            if (!playerStats.discoveredFish.Contains(fish.description))
            {
                playerStats.discoveredFish.Add(fish.description);
            }
            bucketTotal += Mathf.RoundToInt(fish.value * mult);
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
    public void GoEncyclopedia()
    {
        StopAllCoroutines();
        StartCoroutine(FadeOut(1, 3));
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
            print("2");
            hook.shopTutorial = false;
            hook.shop.SetActive(false);
            point.SetActive(false);
        }
        else if (bulkTutorial)
        {
            print("3");
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
            if (SceneManager.GetActiveScene().name == "Main Menu")
                controls.SetActive(!controls.activeSelf);
            else
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
    }
    public void A(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            part2 = true;
            if (SceneManager.GetActiveScene().name == "Main Menu")
            {
                if (controls.activeSelf)
                {
                    if (tutorialOn)
                        tutorialText.text = "Press A to toggle in-game\ntutorial (OFF)";
                    else
                        tutorialText.text = "Press A to toggle in-game\ntutorial (ON)";
                    tutorialOn = !tutorialOn;
                }
                else
                    GoFish();

            }
            else if (SceneManager.GetActiveScene().name == "Game Over")
                GoMenu();
        }
        if (context.canceled)
        {
            part2 = false;
        }
    }
    public void Up(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            part3 = true;
        }
        if (context.canceled)
        {
            part3 = false;
        }
    }
    public void Right(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            part1 = true;
        }
        if (context.canceled)
        {
            part1 = false;
        }
    }

    public static string TimeFormat(float time, bool withDay = false)
    {
        string timer = "";
        if (withDay)
        {
            timer += "Day " + Mathf.CeilToInt(time / 180) + " - ";
        }
        if (time > 3600)
        {
            timer += Mathf.FloorToInt(time / 3600) + ":";
            if (Mathf.FloorToInt(time) % 3600 < 600) timer += "0";
        }
        timer += Mathf.FloorToInt(time / 60) % 60 + ":";
        if (Mathf.FloorToInt(time) % 60 < 10) timer += "0";
        timer += Mathf.FloorToInt(time) % 60;
        return timer;
    }
}
