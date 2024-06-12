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
    //List of all fish, used by other scripts
    public List<GameObject> allFish;

    //Tutorial stuff
    [SerializeField] GameObject controls;
    [SerializeField] TextMeshProUGUI tutorialText;
    static bool tutorialOn;
    public GameObject point;
    public GameObject bulk;
    bool bulkTutorial;

    //Player statistics
    public Fishing hook;
    public PlayerStatsEpic playerStats;
    public WhatTheSavema whatTheSavema;
    
    //Time related things
    public float time = 0;
    public TextMeshProUGUI timer;
    public Image foodBar;

    //Bucket
    public float bucketTotal;
    public TextMeshProUGUI total;
    public Image bucketBar;
    public bool full;
    
    //Update UI only 10 times a second
    float clock;

    //Track if the player wants to cancel fishing
    bool fishCancel;

    //Shop and fishing camera
    public GameObject shoppe;
    public GameObject feesherCam;

    //Pause and scene transition
    public GameObject pause;
    public Image transitionImage;
    Color color;

    //Light control
    public Light2D lt;
    public int bulbLvl;

    //Music
    public MusicControl music;

    //Flying credits controls - unused outside of Main Menu
    public GameObject creditsObject;
    public string[] credits;
    public GameObject thing;

    private void Start()
    {
        //Don't be paused immediately... just in case
        ForceUnpause();

        //Tutorial setup
        tutorialOn = true;
        if (controls != null)
            controls.SetActive(false);
        bulkTutorial = true;

        //Fade into scene
        color = Color.black;
        StartCoroutine(FadeIn(1));
        
        if (SceneManager.GetActiveScene().name == "Fishing")
        {
            //Get the hook and hide some of the further tutorials
            bulk.SetActive(false);
            hook = GameObject.Find("Fishing Hook").GetComponent<Fishing>();
            point.SetActive(false);
        }
        if (SceneManager.GetActiveScene().name == "Main Menu")
        {
            //Spawn the credits blocks, reset the player, and load saves
            StartCoroutine(CreditsSpawning());
            time = -69;
            playerStats.Reset();
            if (whatTheSavema != null)
                whatTheSavema.Load();
        }
        if (SceneManager.GetActiveScene().name == "Game Over")
        {
            //Set up the scores, after sorting by highest
            whatTheSavema.Load();
            whatTheSavema.ArrangeScores();
            whatTheSavema.Save();
        }
    }
    private void Awake()
    {
        if (!tutorialOn && SceneManager.GetActiveScene().name == "Fishing")
        {
            //If you have tutorials off, don't have the tutorials. WOAH
            hook.tutorial = false;
            hook.foodTutorial = false;
            hook.shopTutorial = false;
        }
        allFish.Sort((a, b) => a.GetComponent<Fish>().maxDepth.CompareTo(b.GetComponent<Fish>().maxDepth));
    }

    private void Update()
    {
        //There's a lot of things specific to the fishing scene. I wonder why...
        if (SceneManager.GetActiveScene().name == "Fishing")
        {
            // MAKE TIME PASS
            if (clock < 0.1)
                clock += Time.deltaTime;
            else
            {
                UIUpdate();
                clock = 0;
            }
            time += Time.deltaTime;

            // SHOW THE PLAYER THAT TIME HAS PASSED
            timer.text = TimeFormat(time,true);

            // New day every 3 minutes. Resistant to backwards time travel
            if (time >= playerStats.day * 180)
                playerStats.NewDay();

            //Turtle hunger management
            playerStats.turtleHunger -= Time.deltaTime * 100 / 180;
            foodBar.fillAmount = playerStats.turtleHunger / 100;
            if (playerStats.turtleHunger <= 0)
            {
                GoLose();
                playerStats.finalTime = time;
            }

            //Full bucket management
            if (playerStats.bucket.Count >= playerStats.bucketSize)
                full = true;
        }
        else if (SceneManager.GetActiveScene().name == "Game Over")
        {
            //Keep sorting the scores for no reason. It's PAX's energy bill, not mine
            whatTheSavema.ArrangeScores();
        }

        //Empty bucket total when bucket empty
        if (playerStats.bucket.Count == 0)
            bucketTotal = 0;
    }

    //Coroutine to make the game fade in
    public IEnumerator FadeIn(float speed)
    {
        //Stop pausing multiple times on frame 1 of gameplay. That literally never happens but whatever
        ForceUnpause();
        while (color.a > -0.05f)
        {
            //Fade over "speed" seconds
            color.a -= Time.deltaTime * (1 / speed);
            transitionImage.color = color;
            yield return new WaitForFixedUpdate();
        }
        yield break;
    }

    //Coroutine to make the game fade out
    public IEnumerator FadeOut(float speed, int scene)
    {
        //Me when I frame perfect pause when my turtle dies
        ForceUnpause();
        while (color.a < 1.05f)
        {
            //Fade over "speed" seconds, again
            color.a += Time.deltaTime * (1 / speed);
            transitionImage.color = color;
            yield return new WaitForFixedUpdate();
        }
        //Load scene after completely blacked out (plus a little extra)
        SceneManager.LoadScene(scene);
        yield break;
    }

    //YOU BETTER NOT BE PAUSED WHEN *THIS* FUNCTION GETS CALLED!!!
    public void ForceUnpause()
    {
        //Stop the pause music
        if (SceneManager.GetActiveScene().name == "Fishing")
        {
            music.music.UnPause();
            music.pauseMusic.Stop();
        }

        pause.SetActive(false);
        Time.timeScale = 1;
    }

    //Add a caught fish into the bucket
    public void AddFish(Fish fish, float mult)
    {
        if (playerStats.bucket.Count < playerStats.bucketSize)
        {
            //Add the fish's value to the bucket
            playerStats.BucketAdd(Mathf.RoundToInt(fish.value * mult));
            bucketTotal += Mathf.RoundToInt(fish.value * mult);
            //If we haven't seen this fish before, add it into the discovered list
            if (!playerStats.discoveredFish.Contains(fish.description))
            {
                playerStats.discoveredFish.Add(fish.description);
            }
            //Make sure the UI reflects your new fish
            UIUpdate();
        }
        else
        {
            //Bucket is full. Can we add something that happens in game to tell the player this?
        }
    }

    //Update the UI that doesn't need immediate attention
    public void UIUpdate()
    {
        bucketBar.fillAmount = (float)playerStats.bucket.Count / playerStats.bucketSize;
        total.text = "$" + bucketTotal;

    }

    //Functions to go to specific scenes, with the appropriate fade added \/
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
    //Plus a scene value one just in case
    public void GoScene(int scene)
    {
        StopAllCoroutines();
        StartCoroutine(FadeOut(1, scene));
    }

    //Spawn the little credits boxes in the menu
    IEnumerator CreditsSpawning()
    {
        //Base position
        Vector2 pos = new Vector2(1200, -270);
        int i = 0;
        while (true)
        {
            //Make a credits object, and kill it eventually
            GameObject fish = Instantiate(creditsObject, GameObject.Find("Credits Container").transform);
            fish.GetComponent<Rigidbody2D>().velocity = Vector2.left * Random.Range(25f, 250f);
            Destroy(fish, 60);
            
            //Put the credits into it
            TextMeshProUGUI text = fish.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
            text.text = credits[i];
            i = (i + 1) % credits.Length;

            //Move the spawned fish and set its size to fit the text perfectly
            RectTransform lastFish = fish.GetComponent<RectTransform>();
            pos.y = Random.Range(-260f, -440f);
            lastFish.anchoredPosition = pos;
            lastFish.sizeDelta = new Vector2(text.preferredWidth + 30, text.preferredHeight + 20);

            //Send the fish away to bump into the other ones
            fish.GetComponent<CapsuleCollider2D>().size = lastFish.sizeDelta;
            fish.GetComponent<Rigidbody2D>().mass = Random.Range(0.1f, 100f);
            fish.GetComponent<SplashEffect>().parent = thing;

            //Wait a little bit
            yield return new WaitForSeconds(3);
        }
    }

    //Switch between the shop view and the fishing view.
    void SwitchShop()
    {
        //Reverse active status of cameras and hook
        shoppe.SetActive(!shoppe.activeSelf);
        feesherCam.SetActive(!feesherCam.activeSelf);
        hook.FreezeUnfreeze();

        if (hook.shopTutorial && shoppe.activeSelf)
            point.SetActive(true); //Activate pointing tutorial
        else if (hook.shopTutorial && !shoppe.activeSelf)
        {
            //Disable the first tutorials once we've seen them
            print("2");
            hook.shopTutorial = false;
            hook.shop.SetActive(false);
            point.SetActive(false);
        }
        else if (bulkTutorial)
        {
            //Enable the bulk tutorial once bulk buying is allowed
            print("3");
            bulkTutorial = false;
            bulk.SetActive(true);
        }
        else if (bulk.activeSelf)
            bulk.SetActive(false); //TURN IT OFF
    }

    //B Button
    public void B(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (pause.activeSelf)
            {
                ForceUnpause();
                GoMenu();
            }
            else if (SceneManager.GetActiveScene().name == "Main Menu")
                controls.SetActive(!controls.activeSelf); //Show controls menu in main menu
            else
            {
                //Switch to the shop, or stop reeling
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

    //A Button
    public void A(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (pause.activeSelf)
            {
                ForceUnpause();
            }
            else if (SceneManager.GetActiveScene().name == "Main Menu")
            {
                //If the controls menu is up, toggle tutorials, or start game
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
        }
        
    }

    //Pause button
    public void Pause(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            //Show/hide the menu and stop/start time
            if (pause.activeSelf)
            {
                //Stop the pause music
                music.music.UnPause();
                music.pauseMusic.Stop();
                
                pause.SetActive(false);
                Time.timeScale = 1;
            }
            else
            {
                //Start the pause music and pause everything else
                music.music.Pause();
                music.shopMusic.Stop();
                music.pauseMusic.Play();

                pause.SetActive(true);
                Time.timeScale = 0;
            }
        }
    }

    //Static function to format time. Can also do day thing
    public static string TimeFormat(float time, bool withDay = false)
    {
        //Running string
        string timer = "";

        //Add the day
        if (withDay)
        {
            timer += "Day " + Mathf.CeilToInt(time / 180) + " - ";
        }

        //Hours
        if (time > 3600)
        {
            timer += Mathf.FloorToInt(time / 3600) + ":";
            //If you have <10 minutes, add the leading 0
            if (Mathf.FloorToInt(time) % 3600 < 600) timer += "0";
        }

        //Minutes
        timer += Mathf.FloorToInt(time / 60) % 60 + ":";
        //If you have <10 seconds, add the leading 0
        if (Mathf.FloorToInt(time) % 60 < 10) timer += "0";

        //Seconds
        timer += Mathf.FloorToInt(time) % 60;
        return timer;
    }

    //Calculate what fish should spawn at a given depth
    public List<GameObject> FindFishAtDepth(float depth)
    {
        depth = Mathf.Abs(depth);
        List<GameObject> temp = new();
        foreach (GameObject fish in allFish)
        {
            Fish gamer = fish.GetComponent<Fish>();
            if (gamer.minDepth < depth && depth < gamer.maxDepth)
            {
                temp.Add(fish);
            }
        }
        return temp;
    }

    //Save current stats into the high score system
    public void SaveCurrentStats(string chosenName)
    {
        whatTheSavema.highScores.scores.Add(new(chosenName, playerStats.finalTime, playerStats.points, playerStats.discoveredFish.Count));
        whatTheSavema.ArrangeScores();
        whatTheSavema.Save();
    }
}
