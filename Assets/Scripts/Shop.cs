using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.UI;
public class Shop : MonoBehaviour
{
    //Money and dialogue
    public TextMeshProUGUI textbox;
    public TextMeshProUGUI money;

    //Text for prices
    public TextMeshProUGUI bulbCost;
    public TextMeshProUGUI lineCost;
    public TextMeshProUGUI reelCost;
    public TextMeshProUGUI bucketCost;

    //Game manager
    public GameManager gm;

    //Noises
    public AudioSource[] noises;
    public AudioSource chaching;

    //Coroutine...
    Coroutine holdThatCoroutine;

    //Seagull animation
    public Animator seagull;

    // Unity editor incapable of comprehending such a complex concept such as "array array", so all dialogue arrays are different
    public string[] regularDialogue;
    public string[] browsingDialogue;
    public string[] purchaseDialogue;
    public string[] brokeDialogue;
    public string[] leavingDialogue;
    public string[] leftDialogue;
    public string[] soldDialogue;

    //Highlights of products
    public SpriteRenderer lineButton;
    public SpriteRenderer foodButton;
    public SpriteRenderer bucketButton;
    public SpriteRenderer bulbButton;
    public SpriteRenderer reelButton;
    public SpriteRenderer sellButton;
    Color faded = new Color(1,1,1,(50/255f));
    Color highlight = new Color (1,1,1,1);

    //Dialogue control
    string currentDialogue;
    string remainingDialogue;
    float cawTimer;

    //Hold off input
    bool stopInput;

    //Input Variables
    bool up; 
    bool left;
    bool down;
    bool right;
    bool changed;

    //More dialogue control, this time status checking
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

    //Reference to player stats, for money and such
    public PlayerStatsEpic playerStats;

    private void OnEnable()
    {
        //Clear the highlights
        ResetColors();

        //Enter dialogue
        status = Status.None;
        Dialogue(regularDialogue, Status.Normal);
        StartCoroutine(Typing());
    }

    private void FixedUpdate()
    {
        //Set the numbers for money and price
        money.text = "$" + playerStats.money;
        bulbCost.text = "$" + playerStats.lightbulbCost;
        bucketCost.text = "$" + playerStats.bucketSizeCost;
        reelCost.text = "$" + playerStats.strengthMultUpgradeCost;
        lineCost.text = "$" + playerStats.lineSpeedUpgradeCost;

        //Tick the caw timer
        cawTimer -= Time.deltaTime;

        //Dialogue check in case these variables are misaligned
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

        //Input for pointing
        if (changed)
        {
            switch (true) // figures out what direction the joystick is facing
            {
                case true when (up && left): //Line
                    ResetColors();
                    lineButton.color = highlight;
                    PointToItem(1);
                    break;
                case true when (left && !up && !down): //Food
                    ResetColors();
                    foodButton.color = highlight;
                    PointToItem(0);
                    break;
                case true when (down && left): //Bucket
                    ResetColors();
                    bucketButton.color = highlight;
                    PointToItem(3);
                    break;
                case true when (down && right): //Fish pile
                    ResetColors();
                    sellButton.color = highlight;
                    PointToItem(5);
                    break;
                case true when (right && !up && !down): //Reel
                    ResetColors();
                    reelButton.color = highlight;
                    PointToItem(2);
                    break;
                case true when (up && right): //Light
                    ResetColors();
                    bulbButton.color = highlight;
                    PointToItem(4);
                    break;
                default:
                    //Don't do anything if conflicting directions are input (like a keyboard player mashing all the arrows at once)
                    ResetColors();
                    break;
            }
            changed = false;
        }
    }

    //brooo these colors are so fadedddddd
    void ResetColors()
    {
        lineButton.color = faded;
        foodButton.color = faded;
        bucketButton.color = faded;
        bulbButton.color = faded;
        reelButton.color = faded;
        sellButton.color = faded;
    }

    //Initialization of scrolling dialogue, assuming the dialogue has changed
    void Dialogue(string[] array, Status whatisthisone, int force = -1)
    {
        if (whatisthisone != status)
        {
            status = whatisthisone;
            currentDialogue = "";
            if (force != -1) //Not about forcing the dialogue to change, but forcing which one to pick from the array
                remainingDialogue = array[force];
            else
                remainingDialogue = array[Random.Range(0,array.Length)];
        }
    }

    //Scrolling dialogue, somehow simpler than other stuff we've written
    IEnumerator Typing()
    {
        while (true)
        {
            //Only continue through when we need to talk more
            seagull.Play("talknt");
            yield return new WaitUntil(() => remainingDialogue != null);

            //Sound and animation preparation
            cawTimer = 0.1f;
            seagull.Play("talk");

            //Talk loop
            while (remainingDialogue.Length > 0)
            {
                //Add the next letter into the dialogue box
                currentDialogue += remainingDialogue[0];
                //Remove the first letter from the remaining dialogue
                remainingDialogue = remainingDialogue[1..];
                //CAAAAAAAAWWWWWWW
                if (cawTimer < 0)
                {
                    noises[Random.Range(0, noises.Length)].Play();
                    cawTimer += Random.Range(0.25f, 0.4f);
                }
                //Set text
                textbox.text = currentDialogue;
                //20 letters per second
                yield return new WaitForSeconds(0.02f);
            }
            //Extra caw timer set just in case
            cawTimer = 0.1f;
        }
    }

    //Make the seagull talk about an item
    public void PointToItem(int whichButton)
    {
        if (!stopInput && changed)
        {
            status = Status.None;
            Dialogue(browsingDialogue, Status.LookingAtItem, whichButton);
        }
    }

    //Buy an item when you press A
    public void Confirm(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            switch (true) // figures out what direction the joystick is facing
            {
                case true when (up && left): //Line
                    BuyItem(1);
                    break;
                case true when (left && !up && !down): //Food
                    BuyItem(0);
                    break;
                case true when (down && left): //Bucket
                    BuyItem(3);
                    break;
                case true when (down && right): //Sell Fish
                    playerStats.SellFish();
                    chaching.Play();
                    Dialogue(soldDialogue, Status.SoldFish);
                    break;
                case true when (right && !up && !down): //Reel
                    BuyItem(2);
                    break;
                case true when (up && right): //Light
                    BuyItem(4);
                    break;
                default:
                    //Don't do anything if conflicting directions are input (like a keyboard player mashing all the arrows at once)
                    break;
            }
        }
        if (context.performed && playerStats.day >= 4 && gm.shoppe.activeSelf)
        {
            //If it's day 4 or further, start the coroutine to bulk buy food
            holdThatCoroutine = StartCoroutine(BulkBuyFood());
        }
        if (context.canceled)
        {
            //Stop the bulk coroutine if it exists
            if (playerStats.day >= 4 && holdThatCoroutine != null)
            StopCoroutine(holdThatCoroutine);
        }
    }

    //Bulk food buying coroutine
    IEnumerator BulkBuyFood()
    {
        //Keep buying food EVERY FRAME until you stop holding left and A
        while (left && !up && !down)
        {
            BuyItem(0);
            yield return new WaitForFixedUpdate();
        }
    }

    //Purchase an item and change stats to reflect
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
                            //Enable food tutorial once
                            if (gm.hook.foodTutorial && !gm.hook.shopTutorial)
                            {
                                gm.hook.foodTutorial = false;
                                gm.hook.foodBought = true;
                            }
                        }
                        break;
                    case 1:
                        // fishing line
                        if (playerStats.money < playerStats.lineSpeedUpgradeCost)
                            Dialogue(brokeDialogue, Status.YouArePoor);
                        else
                        {
                            playerStats.money -= playerStats.lineSpeedUpgradeCost;
                            Dialogue(purchaseDialogue, Status.PurchasedItem);
                            chaching.Play();
                            //Speed the player up
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
                            //Get stronk
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
                            //Make the bucket bigger (USES OLD SYSTEM, CAEL MAKE NEW SYSTEM)
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
                            //Enable the light if it's purchased the first time, or make it better
                            if (gm.bulbLvl == 0)
                                gm.lt.enabled = true;
                            else
                                gm.lt.pointLightOuterRadius += 1;
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

    //Directional inputs
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
