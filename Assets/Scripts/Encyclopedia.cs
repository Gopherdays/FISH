using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

public class Encyclopedia : MonoBehaviour
{
    public PlayerStatsEpic playerStats;
    public GameObject[] listOfAllFishInTheGameAGAIN;

    public Image[] leftSidePictures;
    public Image[] leftSideBackgrounds;
    public TextMeshProUGUI[] leftSideNames;
    public int selection;

    public Image rightSideImage;
    public TextMeshProUGUI[] rightSideStats;
    public Sprite blankFish;
    public TextMeshProUGUI pageIndex;

    private void Update()
    {
        for (int i = 0 + ((selection / 8) * 8); i < 8 + ((selection / 8) * 8); i++)
        {
            if (i < listOfAllFishInTheGameAGAIN.Length)
            {
                leftSidePictures[i - ((selection / 8) * 8)].gameObject.SetActive(true);
                leftSideNames[i - ((selection / 8) * 8)].gameObject.SetActive(true);
                leftSidePictures[i - ((selection / 8) * 8)].sprite = listOfAllFishInTheGameAGAIN[i].GetComponent<SpriteRenderer>().sprite;
                if (HasFish(i))
                {
                    leftSidePictures[i - ((selection / 8) * 8)].color = Color.white;
                    leftSideNames[i - ((selection / 8) * 8)].text = listOfAllFishInTheGameAGAIN[i].name;
                }
                else
                {
                    leftSidePictures[i - ((selection / 8) * 8)].color = Color.black;
                    leftSideNames[i - ((selection / 8) * 8)].text = "???";
                }
            }
            else
            {
                leftSidePictures[i - ((selection / 8) * 8)].gameObject.SetActive(false);
                leftSideNames[i - ((selection / 8) * 8)].gameObject.SetActive(false);
            }
        }

        foreach (Image item in leftSideBackgrounds)
        {
            item.enabled = false;
        }
        leftSideBackgrounds[selection%8].enabled = true;
        pageIndex.text = "Page " + ((selection / 8) + 1) + "/" + ((listOfAllFishInTheGameAGAIN.Length / 8) + 1);
    }

    public bool HasFish(int index)
    {
        return (playerStats.discoveredFish.Contains(listOfAllFishInTheGameAGAIN[index].GetComponent<Fish>().description));
    }

    public void DisplayFish(FishData fish)
    {
        rightSideImage.sprite = fish.sprite;
        rightSideImage.SetNativeSize();
        rightSideImage.rectTransform.sizeDelta *= 3;

        rightSideStats[0].text = fish.name;

        rightSideStats[1].text = fish.description;

        switch (fish.speed)
        {
            case <= 0.1f:
                rightSideStats[2].text = "Extremely Fast";
                break;
            case <= 0.2f:
                rightSideStats[2].text = "Very Fast";
                break;
            case <= 0.4f:
                rightSideStats[2].text = "Fast";
                break;
            case <= 0.7f:
                rightSideStats[2].text = "Medium";
                break;
            case <= 1f:
                rightSideStats[2].text = "Slow";
                break;
            default:
                rightSideStats[2].text = "Very Slow";
                break;
        }

        rightSideStats[3].text = fish.escape.ToString() + " m/s";

        switch (fish.turnChance)
        {
            case 0:
                rightSideStats[4].text = "Never";
                break;
            case <= 1:
                rightSideStats[4].text = "Very Unlikely";
                break;
            case <= 3:
                rightSideStats[4].text = "Unlikely";
                break;
            case <= 10:
                rightSideStats[4].text = "Sometimes";
                break;
            case <= 25:
                rightSideStats[4].text = "Often";
                break;
            case <= 40:
                rightSideStats[4].text = "Very Often";
                break;
            default:
                rightSideStats[4].text = "Continuously";
                break;
        }

        switch (fish.skipChance)
        {
            case 0:
                rightSideStats[5].text = "Never";
                break;
            case <= 1:
                rightSideStats[5].text = "Very Unlikely";
                break;
            case <= 3:
                rightSideStats[5].text = "Unlikely";
                break;
            case <= 10:
                rightSideStats[5].text = "Sometimes";
                break;
            case <= 25:
                rightSideStats[5].text = "Often";
                break;
            case <= 40:
                rightSideStats[5].text = "Very Often";
                break;
            default:
                rightSideStats[5].text = "Continuously";
                break;
        }

        rightSideStats[6].text = "Base Value: $" + fish.value;
    }

    public void Left(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            selection = Mathf.Clamp(selection -= 8,0,listOfAllFishInTheGameAGAIN.Length - 1);
        }
    }

    public void Right(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            selection = Mathf.Clamp(selection += 8, 0, listOfAllFishInTheGameAGAIN.Length - 1);
        }
    }

    public void Up(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            selection = Mathf.Clamp(selection -= 1, 0, listOfAllFishInTheGameAGAIN.Length - 1);
        }
    }

    public void Down(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            selection = Mathf.Clamp(selection += 1, 0, listOfAllFishInTheGameAGAIN.Length - 1);
        }
    }

    public void A(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (HasFish(selection))
                DisplayFish(new FishData(listOfAllFishInTheGameAGAIN[selection].GetComponent<Fish>()));
        }
    }
}

public class FishData
{
    public string name;
    public string description;
    public float speed;
    public float escape;
    public int turnChance;
    public int skipChance;
    public int value;
    public Sprite sprite;

    public FishData(string name, string description, float speed, float escape, int turnChance, int skipChance, int value, Sprite sprite)
    {
        this.name = name;
        this.description = description;
        this.speed = speed;
        this.escape = escape;
        this.turnChance = turnChance;
        this.skipChance = skipChance;
        this.value = value;
        this.sprite = sprite;
    }

    public FishData (Fish fish)
    {
        name = fish.gameObject.name;
        description = fish.description;
        speed = fish.speed;
        escape = fish.escape;
        turnChance = fish.turnChance;
        skipChance = fish.skipChance;
        value = fish.value;
        sprite = fish.gameObject.GetComponent<SpriteRenderer>().sprite;
    }
}