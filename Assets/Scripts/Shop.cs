using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Shop : MonoBehaviour
{
    public TextMeshProUGUI textbox;
    public AudioSource[] noises;
    public TextMeshProUGUI money;
    public TextMeshProUGUI food;
    // Unity editor incapable of comprehending such a complex concept such as "array array"
    public string[] regularDialogue;
    public string[] browsingDialogue;
    public string[] purchaseDialogue;
    public string[] brokeDialogue;
    public string[] leavingDialogue;
    public string[] leftDialogue;
    public string[] soldDialogue;
    string currentDialogue;
    string remainingDialogue;
    float cawTimer;
    bool stopInput;
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

    private void Start()
    {
        Dialogue(regularDialogue, Status.Normal);
        StartCoroutine(Typing());
        if (playerStats.candleStatus > 1)
            candleButton.SetActive(false);
        money.text = "$" + playerStats.money;
        food.text = playerStats.turtleFood + "/" + playerStats.turtleHunger;
    }

    private void FixedUpdate()
    {
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
        yield return new WaitUntil(() => remainingDialogue != null);
        while (true)
        {
            if (remainingDialogue != null && remainingDialogue.Length > 0)
            {
                currentDialogue += remainingDialogue[0];
                remainingDialogue = remainingDialogue[1..];
                if (cawTimer < 0)
                {
                    noises[Random.Range(0, noises.Length)].Play();
                    cawTimer += Random.Range(0.15f, 0.3f);
                }
                textbox.text = currentDialogue;
            }
            yield return new WaitForSeconds(0.05f);
        }
        
    }

    public void Leave()
    {
        if (status != Status.TryLeave)
            Dialogue(leavingDialogue, Status.TryLeave);
        else
        {
            Dialogue(leftDialogue, Status.Leave);
            stopInput = true;
            StartCoroutine(ActuallyLeave());
        }
    }
    IEnumerator ActuallyLeave()
    {
        yield return new WaitForSeconds(4);
        GameObject.Find("Game Manager").GetComponent<GameManager>().GoBoat();
    }

    public void SellFish()
    {
        foreach (int item in playerStats.fishPricesAfterDay)
        {
            playerStats.money += item;
        }
        playerStats.bucket.Clear();
        playerStats.fishPricesAfterDay.Clear();
        Dialogue(soldDialogue, Status.SoldFish);
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
                        // turtle food (code multiple things later)
                        if (playerStats.money < 5)
                            Dialogue(brokeDialogue, Status.YouArePoor);
                        else
                        {
                            playerStats.money -= 5;
                            Dialogue(purchaseDialogue, Status.PurchasedItem);
                            playerStats.turtleFood++;
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
                            playerStats.lightTier++;
                            playerStats.lightbulbCost *= 2;
                        }
                        break;
                    case 5:
                        //                                                                                                              <--   candle
                        if (playerStats.candleStatus != 0) { break; }
                        if (playerStats.money < 2)
                            Dialogue(brokeDialogue, Status.YouArePoor);
                        else
                        {
                            playerStats.money -= 2;
                            Dialogue(purchaseDialogue, Status.PurchasedItem);
                            playerStats.candleStatus = 1;
                        }
                        break;
                    default:
                        // cannaeli
                        break;
                }
            }
        }
    }
}
