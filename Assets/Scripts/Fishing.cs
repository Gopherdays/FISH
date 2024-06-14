using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
using TMPro;

public class Fishing : MonoBehaviour
{
    //Player stuff
    public GameManager gm;
    public PlayerStatsEpic stats;

    //Hook controlling
    CircleCollider2D coll;
    Rigidbody2D rb;
    Vector2 origin;
    SpriteRenderer sr;
    public float hookSpeedHorizontal = 2;
    public float hookSpeedVertical = 4;
    float prevY;
    public bool thrown;

    //Fish stats, when reeling
    public GameObject fish;
    public Fish fishFish;
    public float valueMult;
    Vector2 tempV;

    //Reeling UI
    public GameObject indicator;
    public GameObject catcher;
    public TextMeshProUGUI depthText;
    int catcherPos;
    int curCatcherPos;
    int indPos;
    int curIndPos;

    //Reeling minigame position
    bool up;
    bool left;
    bool down;
    bool right;
    List<float> positions = new List<float>();

    //Input whatevers
    bool confirm;
    bool once;
    bool changed;
    bool bulk;
    Vector2 temp = new Vector2();

    //Tutorials
    public GameObject feed;
    public GameObject move;
    public GameObject cast;
    public GameObject aim;
    public GameObject shop;
    public bool tutorial;
    public bool shopTutorial;
    public bool foodTutorial;
    public bool foodBought;

    //Last fish
    public TextMeshProUGUI lastFish;

    //Fish stats
    public float speed; 
    public float escape;
    public int turnChance;
    public int skipChance;
    public int value;
    public int baseValue;
    public float distance;
    float timer;
    bool direction;
    bool skip;

    //Beautify
    public GameObject splash;
    CameraScript cs;
    FishingLine fl;
    CinemachineVirtualCamera cam;

    //HUD
    public TextMeshProUGUI foodCount;
    GameObject fishingUI;
    GameObject depth;
    bool hooking;
    public bool fishing;
    bool reset;

    //Audio
    private AudioSource source;
    public AudioClip clip1, clip2, clip3, clip4;
    public AudioClip[] sounds;
    public AudioClip[] sounds2;

    private void Start()
    {
        //OMEGA INITIALIZATION
        //This is basically just all setting up

        //Get components and find things
        coll = GetComponent<CircleCollider2D>();
        fishingUI = GameObject.Find("Fishing UI");
        depth = GameObject.Find("Depth Meter");
        cam = GameObject.Find("Virtual Camera").GetComponent<CinemachineVirtualCamera>();
        fl = GameObject.Find("Line Renderer").GetComponent<FishingLine>();
        cs = cam.gameObject.GetComponent<CameraScript>();
        source = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody2D>();
        FreezeUnfreeze();
        sr = GetComponent<SpriteRenderer>();

        //Set actives
        fishingUI.SetActive(false);
        depth.SetActive(false);
        move.SetActive(false);
        aim.SetActive(false);
        shop.SetActive(false);
        feed.SetActive(false);

        //Don't move at the start
        up = false;
        left = false;
        down = false;
        right = false;
        fishing = false;
        hooking = true;

        //Tutorials
        tutorial = true;
        shopTutorial = true;
        foodTutorial = true;

        //Vector 2s and position related things
        origin = GameObject.Find("Player").transform.position;
        temp = new Vector2(0, 0);
        tempV = new Vector2(-7, -5);
        prevY = transform.position.y;
        indPos = Random.Range(2, 7);
        curIndPos = indPos;
        hookSpeedHorizontal = 2 * stats.lineSpeedHorizontal;
        hookSpeedVertical = 4 * stats.lineSpeedVertical;
        for (int i = 0; i < 8; i++)
        {
            positions.Add(i * 45);
        }
        indicator.transform.rotation = Quaternion.Euler(0, 0, positions[indPos]);
    }

    private void Update()
    {
        // A Button while casted and outside of the shop
        if (thrown && confirm && !gm.shoppe.activeSelf)
        {
            //Only confirm once if bulk is disabled
            if (gm.playerStats.day < 4 || !bulk)
                confirm = false;
            //If the player has food, play the food sound (PlayerStats.Feed() bool does the feeding already)
            if (gm.playerStats.Feed())
            {
                source.clip = sounds2[Random.Range(0, sounds2.Length)];
                source.Play();
            }
        }
        
        // Direction change while casted
        if (thrown && changed)
        {
            changed = false;
            //Deal with hook-based tutorials
            if (move.activeSelf)
            {
                StartCoroutine(WaitHook(move));
                StartCoroutine(WaitMove(move));
            }
            if (feed.activeSelf)
            {
                StartCoroutine(WaitHook(feed));
                StartCoroutine(WaitMove(feed));
            }

            //Calculate movement direction
            switch (true)
            {
                case true when (up && !left && !right): //North
                    catcherPos = 0;
                    temp = Vector2.up;
                    break;
                case true when (up && left): //Northwest
                    catcherPos = 1;
                    temp.y = 1;
                    temp.x = -1;
                    break;
                case true when (left && !up && !down): //West
                    catcherPos = 2;
                    temp = Vector2.left;
                    break;
                case true when (down && left): //Southwest
                    catcherPos = 3;
                    temp.y = -1;
                    temp.x = -1;
                    break;
                case true when (down && !left && !right): //South
                    catcherPos = 4;
                    temp = Vector2.down;
                    break;
                case true when (down && right): //Southeast
                    catcherPos = 5;
                    temp.y = -1;
                    temp.x = 1;
                    break;
                case true when (right && !up && !down): //East
                    catcherPos = 6;
                    temp = Vector2.right;
                    break;
                case true when (up && right): //Northeast
                    catcherPos = 7;
                    temp.y = 1;
                    temp.x = 1;
                    break;
                default: //Do nothing if some keyboard player is mashing weird button combos
                    temp = Vector2.zero;
                    break;
            }
        }

        //Stuff if you're not casted
        else if (!thrown)
        {

            //A button to cast
            if (confirm)
            {
                confirm = false;
                //Get rid of the tutorial
                cast.SetActive(false);

                //Disable shop??
                if (shop.activeSelf)
                    shop.SetActive(false);
                
                //Prep movement/feed tutorials
                if (tutorial)
                    StartCoroutine(WaitForInWater(move));
                if (foodBought && foodTutorial && !tutorial)
                    StartCoroutine(WaitForInWater(feed));

                //Actually throw hook
                thrown = true;
                FreezeUnfreeze();
                rb.AddForce(new Vector2(Random.Range(0.25f, 1.25f) * -300, Random.Range(0.5f, 1.25f) * 300));
                source.clip = clip4;
                source.Play();
            }
        }
    }

    private void FixedUpdate()
    {
        // Not reeling in a fish
        if (hooking)
        {
            //If hook has entered the water
            if (transform.position.y < 0 && prevY >= 0)
            {
                //Remove X velocity
                rb.velocity = new Vector2(0,rb.velocity.y);
                
                //Cam follow hook
                cam.Follow = transform;

                //Splash sound
                source.clip = clip1;
                source.Play();
            }

            //Only move if you're under the water
            if (thrown && transform.position.y <= 0)
            {
                temp = temp.normalized;
                temp.y *= stats.lineSpeedVertical;
                temp.x *= stats.lineSpeedHorizontal;
                rb.AddForce(temp * 20);
            }

            //Set previous position
            prevY = transform.position.y;
        }

        // Reeling in a fish
        else if (fishing && !reset)
        {
            //Advance timer unless the fish is supposed to move
            if (timer < speed)
                timer += Time.deltaTime;
            else
            {
                //FISH MOVE INDICATOR
                indPos = NewPos(indPos);
                timer = 0;
            }

            //Fish position has changed, update rotation
            if (indPos != curIndPos)
            {
                indicator.transform.rotation = Quaternion.Euler(0, 0, positions[indPos]);
                curIndPos = indPos;
            }

            //Player position has changed, update rotation
            if (catcherPos != curCatcherPos)
            {
                catcher.transform.rotation = Quaternion.Euler(0, 0, positions[catcherPos]);
                curCatcherPos = catcherPos;
            }

            //Player is right on that fish
            if (catcherPos == indPos)
            { 
                //Play high reeling
                if (!source.isPlaying)
                {
                    source.clip = sounds[Random.Range(0, sounds.Length)]; 
                    source.Play();
                }
                distance -= stats.strengthMult * Time.deltaTime;
               
            }

            //Player is sorta on that fish
            else if (FixIndexOverflow(catcherPos - 1) == indPos || FixIndexOverflow(catcherPos + 1) == indPos)
            {
                //Only half as effective
                distance -= stats.strengthMult * Time.deltaTime * 0.5f;
            }

            //Player is off of the fish
            else
            {
                //WE'RE LOSIN' EM
                distance += escape * Time.deltaTime;
                if (!source.isPlaying)
                {
                    source.clip = clip3;
                    source.Play();
                }
            }

            //Update depth
            depthText.text = (int)distance + "m";

            //Start the bring-up scene if not done already and minigame won
            if (distance <= 0 && fish.transform.position.y == -5)
                reset = true;

        }

        //Bring up fish cutscene
        else if (reset)
        {
            if (fish.transform.position.y <= -1.5f)
            {
                tempV.y += Time.deltaTime * 1.5f;
                fish.transform.position = tempV;
            }
            else
            {
                reset = false;
                fishingUI.SetActive(false);
                depth.SetActive(false);
                if (tutorial)
                {
                    tutorial = false;
                    aim.SetActive(false);
                }
                print(fishFish);
                gm.AddFish(fishFish,valueMult);
                Switch();
            }
        }
        foodCount.text = stats.food.ToString();
    }

    //Switch between fishing or reeling
    void Switch()
    {
        if (hooking)
        {
            //Measure fish related values
            distance = Vector2.Distance(fish.transform.position, origin);
            if (fish.transform.localScale.x > 0)
                fish.transform.SetPositionAndRotation(new Vector2(-7, -5), Quaternion.Euler(0, 0, 90));
            else if (fish.transform.localScale.x < 0)
                fish.transform.SetPositionAndRotation(new Vector2(-7, -5), Quaternion.Euler(0, 0, -90));
            fish.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
            fish.GetComponent<SpriteRenderer>().sortingOrder = 420;
            valueMult = (100 + distance) / 100;
            fl.hook = fish;

            //Camera and UI
            cs.Shake(100);
            cam.Follow = GameObject.Find("Player").transform;
            fishingUI.SetActive(true);
            depth.SetActive(true);
            if (tutorial)
                aim.SetActive(true);
            
            //Get the hook back to the player
            transform.position = origin;
            FreezeUnfreeze();

            //Bools
            once = false;
            sr.enabled = false;
            hooking = false;
            fishing = true;
        }
        else if (fishing)
        {
            StartCoroutine(WaitForAnimation());
        }
    }

    //Show off that fish
    IEnumerator WaitForAnimation()
    {
        yield return new WaitForSeconds(1);

        //UI updating
        lastFish.text = "Last Fish Caught:\n" + Mathf.Round(baseValue + Random.Range(-0.25f * baseValue, 0.25f * baseValue)) + "lb " + fish.name.Remove(fish.name.Length - 7);
        if (shopTutorial)
            shop.SetActive(true);

        //Other things
        stats.points += fish.GetComponent<Fish>().points;
        indPos = Random.Range(2, 7);
        tempV.y = -5;
        catcherPos = 1;
        curCatcherPos = 1;

        //Go back to before throw
        cam.Follow = GameObject.Find("Player").transform;
        FreezeUnfreeze();
        fl.hook = gameObject;
        sr.enabled = true;
        fishing = false;
        hooking = true;
        thrown = false;

        //KILL FISH
        Destroy(fish);
    }
    
    //Hook hit the fish, get the fish and do the fishing fish fish fishing the fish up fish the up fish up the fish
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Fish"))
        {
            //Get fish and stats
            fish = collision.gameObject;
            fishFish = fish.GetComponent<Fish>();
            speed = fishFish.speed;
            escape = fishFish.escape;
            turnChance = fishFish.turnChance;
            skipChance = fishFish.skipChance;
            baseValue = fishFish.value;

            //Play sound and switch
            source.clip = clip2;
            source.Play();
            Switch();
        }
    }

    //Fish choosing a new position
    int NewPos(int position)
    {
        if (Random.Range(1, 101) <= turnChance)
            direction = !direction;

        if (Random.Range(1, 101) <= skipChance)
            skip = true;

        int temp;
        temp = position;
        if (direction && skip)
            temp += 2;
        else if (direction && !skip)
            temp++;
        else if (!direction && skip)
            temp -= 2;
        else
            temp--;
        temp = FixIndexOverflow(temp);
        skip = false;
        return temp;
    }

    //Jake should probably find a better way to do this part
    int FixIndexOverflow(int temp)
    {
        switch (temp)
        {
            case 8:
                temp = 0;
                break;

            case 9:
                temp = 1;
                break;

            case -1:
                temp = 7;
                break;

            case -2:
                temp = 6;
                break;

            default:
                break;
        }
        return temp;
    }

    //Tutorial waiting things
    IEnumerator WaitForInWater(GameObject obj)
    {
        yield return new WaitUntil(() => prevY > 0 && transform.position.y <= 0);
        obj.SetActive(true);
    }
    IEnumerator WaitHook(GameObject obj)
    {
        yield return new WaitUntil(() => fishing);
        if (obj.activeSelf)
        {
            obj.SetActive(false);
        }
    }
    IEnumerator WaitMove(GameObject obj)
    {
        yield return new WaitForSeconds(3);
        if (obj.activeSelf)
        {
            obj.SetActive(false);
        }
    }
    
    //Freeze or unfreeze the hook so you don't catch while shopping
    public void FreezeUnfreeze()
    {
        if (gm.shoppe.activeSelf)
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
        else
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        coll.enabled = !coll.enabled;
        if (rb.position.y >= 0)
        {
            thrown = false;
        }
    }

    //InputSystem stuff
    public void Up(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            up = true;
            changed = true;
        }
        if (context.canceled)
        {
            up = false;
            changed = true;
        }
    }
    public void Left(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            left = true;
            changed = true;
        }
        if (context.canceled)
        {
            left = false;
            changed = true;
        }
    }
    public void Down(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            down = true;
            changed = true;
        }
        if (context.canceled)
        {
            down = false;
            changed = true;
        }
    }
    public void Right(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            right = true;
            changed = true;
        }
        if (context.canceled)
        {
            right = false;
            changed = true;
        }
    }
    public void A(InputAction.CallbackContext context)
    {
        if (context.started && !gm.shoppe.activeSelf && !gm.pause.activeSelf)
        {
            confirm = true;
            bulk = true;
        }
        if (context.canceled)
            bulk = false;
    }
    public void B(InputAction.CallbackContext context)
    {
        if (context.started && fishing)
        {
            //Only release fish when B is pressed twice
            if (!once)
                once = true;
            else
            {
                once = false;
                fishingUI.SetActive(false);
                depth.SetActive(false);
                if (tutorial)
                {
                    tutorial = false;
                    aim.SetActive(false);
                }
                Switch();
            }
        }
    }
}