using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.UI;
public class Shop : MonoBehaviour
{
    public TextMeshProUGUI textbox;
    public TextMeshProUGUI money;

    public TextMeshProUGUI bulbCost;
    public TextMeshProUGUI lineCost;
    public TextMeshProUGUI reelCost;
    public TextMeshProUGUI bucketCost;

    public GameManager gm;
    public AudioSource[] noises;
    public AudioSource chaching;

    public Animator seagull;

    // Unity editor incapable of comprehending such a complex concept such as "array array"
    public string[] regularDialogue;
    public string[] browsingDialogue;
    public string[] purchaseDialogue;
    public string[] brokeDialogue;
    public string[] leavingDialogue;
    public string[] leftDialogue;
    public string[] soldDialogue;

    public SpriteRenderer lineButton;
    public SpriteRenderer foodButton;
    public SpriteRenderer bucketButton;
    public SpriteRenderer bulbButton;
    public SpriteRenderer reelButton;
    public SpriteRenderer sellButton;
    Color faded = new Color(1,1,1,(50/255f));
    Color highlight = new Color (1,1,1,1);

    string currentDialogue;
    string remainingDialogue;
    float cawTimer;
    bool stopInput;

    bool up; // input variables
    bool left;
    bool down;
    bool right;
    bool confirm;
    bool changed;
    int dir;

    enum Status
    {
        None,
        Normal,
        LookingAtItem,
        PurchasedItem,
        YouArePoor,
        SoldFish,
        TryLeave,
        Leave
    }
    Status status = Status.None;

    public PlayerStatsEpic playerStats;
    public GameObject candleButton;

    private void OnEnable()
    {
        ResetColors();
        status = Status.None;
        Dialogue(regularDialogue, Status.Normal);
        StartCoroutine(Typing());
        if (playerStats.candleStatus > 1)
            candleButton.SetActive(false);
    }

    private void FixedUpdate()
    {
        money.text = "$" + playerStats.money;

        bulbCost.text = "$" + playerStats.lightbulbCost;
        bucketCost.text = "$" + playerStats.bucketSizeCost;
        reelCost.text = "$" + playerStats.strengthMultUpgradeCost;
        lineCost.text = "$" + playerStats.lineSpeedUpgradeCost;

        cawTimer -= Time.deltaTime;
        switch (status)
        {
            case Status.Normal:
                Dialogue(regularDialogue, Status.Normal);
                break;
            case Status.LookingAtItem:
                Dialogue(browsingDialogue, Status.LookingAtItem);
                break;
            case Status.PurchasedItem:
                Dialogue(purchaseDialogue, Status.PurchasedItem);
                break;
            case Status.TryLeave:
                Dialogue(leavingDialogue, Status.TryLeave);
                break;
            case Status.Leave:
                Dialogue(leftDialogue, Status.Leave);
                break;
            case Status.YouArePoor:
                Dialogue(brokeDialogue, Status.YouArePoor);
                break;
            default:
                break;
        }
        if (changed)
        {
            switch (true) // figures out what direction the joystick is facing
            {
                case true when (up && left): //northwest
                    ResetColors();
                    lineButton.color = highlight;
                    PointToItem(1);
                    break;
                case true when (left && !up && !down): //west
                    ResetColors();
                    foodButton.color = highlight;
                    PointToItem(0);
                    break;
                case true when (down && left): //southwest
                    ResetColors();
                    bucketButton.color = highlight;
                    PointToItem(3);
                    break;
                case true when (down && right): //southeast
                    ResetColors();
                    sellButton.color = highlight;
                    PointToItem(5);
                    break;
                case true when (right && !up && !down): //east
                    ResetColors();
                    reelButton.color = highlight;
                    PointToItem(2);
                    break;
                case true when (up && right): //northeast
                    ResetColors();
                    bulbButton.color = highlight;
                    PointToItem(4);
                    break;
                default:
                    ResetColors();
                    break;
            }
            changed = false;
        }
    }

    void ResetColors()
    {
        lineButton.color = faded;
        foodButton.color = faded;
        bucketButton.color = faded;
        bulbButton.color = faded;
        reelButton.color = faded;
        sellButton.color = faded;
    }

    void Dialogue(string[] array, Status whatisthisone, int force = -1)
    {
        if (whatisthisone != status)
        {
            status = whatisthisone;
            currentDialogue = "";
            if (force != -1)
                remainingDialogue = array[force];
            else
                remainingDialogue = array[Random.Range(0,array.Length)];
        }
    }

    IEnumerator Typing()
    {
        while (true)
        {
            seagull.Play("talknt");
            yield return new WaitUntil(() => remainingDialogue != null);
            cawTimer = 0.1f;
            seagull.Play("talk");
            while (remainingDialogue.Length > 0)
            {
                currentDialogue += remainingDialogue[0];
                remainingDialogue = remainingDialogue[1..];
                if (cawTimer < 0)
                {
                    noises[Random.Range(0, noises.Length)].Play();
                    cawTimer += Random.Range(0.25f, 0.4f);
                }
                textbox.text = currentDialogue;
                yield return new WaitForSeconds(0.05f);
            }
            cawTimer = 0.1f;
        }
    }

    public void PointToItem(int whichButton)
    {
        if (!stopInput && changed)
        {
            status = Status.None;
            Dialogue(browsingDialogue, Status.LookingAtItem, whichButton);
        }
    }

    public void Confirm(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            switch (true) // figures out what direction the joystick is facing
            {
                case true when (up && left): //northwest
                    BuyItem(1);
                    break;
                case true when (left && !up && !down): //west
                    BuyItem(0);
                    break;
                case true when (down && left): //southwest
                    BuyItem(3);
                    break;
                case true when (down && right): //southeast
                    playerStats.SellFish();
                    chaching.Play();
                    Dialogue(soldDialogue, Status.SoldFish);
                    break;
                case true when (right && !up && !down): //east
                    BuyItem(2);
                    break;
                case true when (up && right): //northeast
                    BuyItem(4);
                    break;
                default:
                    break;
            }
        }
    }

    public void BuyItem(int whichButton)
    {
        if (!stopInput)
        {
            if (status != Status.LookingAtItem)
                Dialogue(browsingDialogue, Status.LookingAtItem, whichButton);
            else
            {
                switch (whichButton)
                {
                    case 0:
                        // turtle food
                        if (playerStats.money < 5)
                            Dialogue(brokeDialogue, Status.YouArePoor);
                        else
                        {
                            playerStats.money -= 5;
                            Dialogue(purchaseDialogue, Status.PurchasedItem);
                            playerStats.food++;
                            chaching.Play();
                            if (gm.hook.foodTutorial && !gm.hook.shopTutorial)
                            {
                                gm.hook.foodTutorial = true;
                                gm.hook.foodBought = true;
                            }
                        }
                        break;
                    case 1:
                        //fhishg line
                        if (playerStats.money < playerStats.lineSpeedUpgradeCost)
                            Dialogue(brokeDialogue, Status.YouArePoor);
                        else
                        {
                            playerStats.money -= playerStats.lineSpeedUpgradeCost;
                            Dialogue(purchaseDialogue, Status.PurchasedItem);
                            chaching.Play();
                            playerStats.lineSpeedVertical *= 1.5f;
                            playerStats.lineSpeedHorizontal *= 1.25f;
                            playerStats.lineSpeedUpgradeCost *= 2;
                        }
                        break;
                    case 2:
                        // fishing reel
                        if (playerStats.money < playerStats.strengthMultUpgradeCost)
                            Dialogue(brokeDialogue, Status.YouArePoor);
                        else
                        {
                            playerStats.money -= playerStats.strengthMultUpgradeCost;
                            Dialogue(purchaseDialogue, Status.PurchasedItem);
                            chaching.Play();
                            playerStats.strengthMult++;
                            playerStats.strengthMultUpgradeCost *= 2;
                        }
                        break;
                    case 3:
                        // bucket
                        if (playerStats.money < playerStats.bucketSizeCost)
                            Dialogue(brokeDialogue, Status.YouArePoor);
                        else
                        {
                            playerStats.money -= playerStats.bucketSizeCost;
                            Dialogue(purchaseDialogue, Status.PurchasedItem);
                            chaching.Play();
                            playerStats.bucketSize += 5;
                            playerStats.bucketSizeCost *= 2;
                        }
                        break;
                    case 4:
                        // lightbulb
                        if (playerStats.money < playerStats.lightbulbCost)
                            Dialogue(brokeDialogue, Status.YouArePoor);
                        else
                        {
                            playerStats.money -= playerStats.lightbulbCost;
                            Dialogue(purchaseDialogue, Status.PurchasedItem);
                            chaching.Play();
                            if (gm.bulbLvl == 0)
                                gm.light.enabled = true;
                            else
                                gm.light.pointLightOuterRadius += 1;
                            gm.bulbLvl++;
                            playerStats.lightbulbCost *= 2;
                        }
                        break;
                    default:
                        // nicholasagna
                        break;
                }
            }
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
}
