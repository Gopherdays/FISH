using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Shop : MonoBehaviour
{
    public TextMeshProUGUI textbox;
    public AudioSource[] noises;
    // Unity editor incapable of comprehending such a complex concept such as "array array"
    public string[] regularDialogue;
    public string[] browsingDialogue;
    public string[] purchaseDialogue;
    public string[] brokeDialogue;
    public string[] leavingDialogue;
    public string[] leftDialogue;
    string currentDialogue;
    string remainingDialogue;
    float cawTimer;
    enum Status
    {
        None,
        Normal,
        LookingAtItem,
        PurchasedItem,
        YouArePoor,
        TryLeave,
        Leave
    }
    Status status = Status.None;

    public PlayerStatsEpic playerStats;

    private void Start()
    {
        Dialogue(regularDialogue, Status.Normal);
        StartCoroutine(Typing());
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

    void Dialogue(string[] array, Status whatisthisone)
    {
        if (whatisthisone != status)
        {
            status = whatisthisone;
            currentDialogue = "";
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

    public void LineUpgradePressed()
    {
        if (status != Status.LookingAtItem)
            Dialogue(browsingDialogue, Status.LookingAtItem);
        else if (playerStats.money < playerStats.lineSpeedUpgradeCost)
        {
            Dialogue(brokeDialogue, Status.YouArePoor);
        }
        else
        {

        }
    }
}
