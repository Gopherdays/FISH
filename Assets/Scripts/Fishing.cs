using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
using UnityEngine.UI;
using TMPro;

public class Fishing : MonoBehaviour
{
    GameObject player;
    public GameObject fish;
    public GameObject indicator;
    public GameObject catcher;
    public TextMeshProUGUI depthText;
    Vector2 tempV;
    int catcherPos;
    int curCatcherPos;
    int indPos;
    int curIndPos;
    List<float> positions = new List<float>();

    bool up;
    bool left;
    bool down;
    bool right;
    bool confirm;
    bool changed;
    Vector2 temp = new Vector2();

    public GameObject move;
    public GameObject cast;
    public GameObject aim;
    public GameObject point;
    public GameObject shop;
    bool tutorial;
    bool shopTutorial;
    
    public GameManager gm;
    public float speed; 
    public float escape;
    public int turnChance;
    public int skipChance;
    public int value;
    public float distance;
    float timer;
    bool direction;
    bool skip;

    public GameObject splash;
    public Transform origin;
    public PlayerStatsEpic stats;
    Rigidbody2D rb;
    CameraScript cs;
    FishingLine fl;
    SpriteRenderer sr;
    CinemachineVirtualCamera cam;
    public float hookSpeedHorizontal = 2;
    public float hookSpeedVertical = 4;
    float prevY;
    public bool thrown;

    public TextMeshProUGUI foodCount;
    GameObject fishingUI;
    GameObject depth;
    bool hooking;
    public bool fishing;
    bool reset;

    private AudioSource source;
    public AudioClip clip1, clip2, clip3;
    public AudioClip[] sounds;

    private void Start()
    {
        fishingUI = GameObject.Find("Fishing UI");
        depth = GameObject.Find("Depth Meter");
        player = GameObject.Find("Player");
        cam = GameObject.Find("Virtual Camera").GetComponent<CinemachineVirtualCamera>();
        fl = GameObject.Find("Line Renderer").GetComponent<FishingLine>();
        cs = cam.gameObject.GetComponent<CameraScript>();
        source = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        fishingUI.SetActive(false);
        depth.SetActive(false);
        move.SetActive(false);
        aim.SetActive(false);
        up = false;
        left = false;
        down = false;
        right = false;
        fishing = false;
        hooking = true;
        tutorial = true;
        shopTutorial = false;
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

        if (thrown && changed)
        {
            changed = false;
            if (tutorial && move.activeSelf)
            {
                StartCoroutine(WaitHook());
                StartCoroutine(WaitMove());
            }
            switch (true)
            {
                case true when (up && !left && !right):
                    catcherPos = 0;
                    temp = Vector2.up;
                    break;
                case true when (up && left):
                    catcherPos = 1;
                    temp.y = 1;
                    temp.x = -1;
                    break;
                case true when (left && !up && !down):
                    catcherPos = 2;
                    temp = Vector2.left;
                    break;
                case true when (down && left):
                    catcherPos = 3;
                    temp.y = -1;
                    temp.x = -1;
                    break;
                case true when (down && !left && !right):
                    catcherPos = 4;
                    temp = Vector2.down;
                    break;
                case true when (down && right):
                    catcherPos = 5;
                    temp.y = -1;
                    temp.x = 1;
                    break;
                case true when (right && !up && !down):
                    catcherPos = 6;
                    temp = Vector2.right;
                    break;
                case true when (up && right):
                    catcherPos = 7;
                    temp.y = 1;
                    temp.x = 1;
                    break;
                default:
                    temp = Vector2.zero;
                    break;
            }
        }
        else if (!thrown)
        {
            if (transform.position != origin.transform.position)
                transform.position = origin.transform.position;
            if (rb.velocity != Vector2.zero)
                rb.velocity = Vector2.zero;
            if (confirm)
            {
                confirm = false;
                cast.SetActive(false);
                if (tutorial)
                    StartCoroutine(WaitForInWater());
                ThrowHook();
            }
        }
    }

    private void FixedUpdate()
    {
        if (hooking)
        {
            if (transform.position.y < 0 && prevY >= 0)
            {
                rb.velocity = new Vector2(0,rb.velocity.y);
                cam.Follow = transform;
                for (int i = 0; i < Random.Range(40, 61); i++)
                {
                    GameObject g = Instantiate(splash, transform.position, Quaternion.identity);
                    float rand = Random.Range(0.25f, 0.35f);
                    g.transform.localScale = new Vector2(rand,rand);
                    float xVal = Random.Range(-0.25f, 0.25f);
                    g.GetComponent<Rigidbody2D>().AddForce(new Vector2(xVal, 2 + Random.Range(0f, 1f) - Mathf.Abs(2 * xVal)) * (30 + Random.Range(0, 50)));
                    Destroy(g, 3);
                }
                source.clip = clip1;
                source.Play();
            }

            if (thrown && transform.position.y <= 0)
            {
                temp = temp.normalized;
                temp.y *= stats.lineSpeedVertical;
                temp.x *= stats.lineSpeedHorizontal;
                rb.AddForce(temp * 20);
            }
            prevY = transform.position.y;
        }

        else if (fishing && !reset)
        {
            if (timer < speed)
                timer += Time.deltaTime;
            else
            {
                indPos = NewPos(indPos);
                timer = 0;
               
            }

            if (indPos != curIndPos)
            {
                indicator.transform.rotation = Quaternion.Euler(0, 0, positions[indPos]);
                curIndPos = indPos;
            }

            if (catcherPos != curCatcherPos)
            {
                catcher.transform.rotation = Quaternion.Euler(0, 0, positions[catcherPos]);
                curCatcherPos = catcherPos;
            }

            if (catcherPos == indPos)
            { 
                
                if (!source.isPlaying)
                {
                    source.clip = sounds[Random.Range(0, sounds.Length)]; 
                    source.Play();
                }
                distance -= stats.strengthMult * Time.deltaTime;
               
            }

            else if (FixIndexOverflow(catcherPos - 1) == indPos || FixIndexOverflow(catcherPos + 1) == indPos)
            {
                distance -= stats.strengthMult * Time.deltaTime * 0.5f;
                
            }

            else
            {
                distance += escape * Time.deltaTime;
                if (!source.isPlaying)
                {
                    source.clip = clip3;
                    source.Play();
                }
            }

            depthText.text = (int)distance + "m";
            if (distance <= 0 && fish.transform.position.y == -5)
                reset = true;

        }
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
                Switch();
            }
        }
        foodCount.text = stats.food.ToString();
    }

    void Switch()
    {
        if (hooking)
        {
            cs.Shake(100);
            cam.Follow = player.transform;
            fishingUI.SetActive(true);
            depth.SetActive(true);
            if (tutorial)
                aim.SetActive(true);
            if (fish.transform.localScale.x > 0)
                fish.transform.SetPositionAndRotation(new Vector2(-7, -5), Quaternion.Euler(0, 0, 90));
            else if (fish.transform.localScale.x < 0)
                fish.transform.SetPositionAndRotation(new Vector2(-7, -5), Quaternion.Euler(0, 0, -90));
            transform.position = player.transform.position;
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
            fish.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
            fish.GetComponent<SpriteRenderer>().sortingOrder = 420;
            distance = Vector2.Distance(fish.transform.position, player.transform.position);
            value = Mathf.RoundToInt(fish.GetComponent<Fish>().value * (100 + distance));
            fl.hook = fish;
            sr.enabled = false;
            hooking = false;
            fishing = true;
        }
        else if (fishing)
        {
            StartCoroutine(WaitForAnimation());
        }
    }

    IEnumerator WaitForAnimation()
    {
        yield return new WaitForSeconds(1);

        print("Yay! You caught: " + fish.name.Remove(fish.name.Length - 7));
        rb.constraints = RigidbodyConstraints2D.None;
        fl.hook = gameObject;
        cam.Follow = GameObject.Find("Player").transform;
        gm.AddFish(fish.GetComponent<Fish>().value);
        stats.points += fish.GetComponent<Fish>().points;
        indPos = Random.Range(2, 7);
        tempV.y = -5;
        catcherPos = 1;
        curCatcherPos = 1;
        sr.enabled = true;
        fishing = false;
        hooking = true;
        thrown = false;
        Destroy(fish);
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Fish"))
        {
            fish = collision.gameObject;
            Fish feesh = fish.GetComponent<Fish>();
            speed = feesh.speed;
            escape = feesh.escape;
            turnChance = feesh.turnChance;
            skipChance = feesh.skipChance;
            source.clip = clip2;
            source.Play();
            Switch();
        }
    }


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

    public void ThrowHook()
    {
        thrown = true;
        rb.AddForce(new Vector2(Random.Range(0.25f, 1.25f) * -150, Random.Range(0.5f, 1.25f) * 150));
    }

    IEnumerator WaitForInWater()
    {
        yield return new WaitUntil(() => prevY > 0 && transform.position.y <= 0);
        move.SetActive(true);
    }

    IEnumerator WaitHook()
    {
        yield return new WaitUntil(() => fishing);
        if (move.activeSelf)
        {
            move.SetActive(false);
        }
    }

    IEnumerator WaitMove()
    {
        yield return new WaitForSeconds(3);
        if (move.activeSelf)
        {
            move.SetActive(false);
        }
    }
    
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
        if (context.started)
        {
            confirm = true;
        }
    }
}
