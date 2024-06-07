using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

public class Encyclopedia : MonoBehaviour
{
    public PlayerStatsEpic playerStats;
    public GameManager gm;

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
        //Fish displaying stuff
        for (int i = 0 + ((selection / 8) * 8); i < 8 + ((selection / 8) * 8); i++)
        {
            if (i < gm.allFish.Count)
            {
                //Show the section as the corresponding fish
                //Dividing then multiplying by 8 actually has a purpose, because integer division rounds down for free
                leftSidePictures[i - ((selection / 8) * 8)].gameObject.SetActive(true);
                leftSideNames[i - ((selection / 8) * 8)].gameObject.SetActive(true);
                leftSidePictures[i - ((selection / 8) * 8)].sprite = gm.allFish[i].GetComponent<SpriteRenderer>().sprite;
                if (HasFish(i))
                {
                    //If you have the fish, actually show it
                    leftSidePictures[i - ((selection / 8) * 8)].color = Color.white;
                    leftSideNames[i - ((selection / 8) * 8)].text = gm.allFish[i].name;
                }
                else
                {
                    //Mysterious fish...
                    leftSidePictures[i - ((selection / 8) * 8)].color = Color.black;
                    leftSideNames[i - ((selection / 8) * 8)].text = "???";
                }
            }
            else
            {
                //Hide boxes that go past the max fish
                leftSidePictures[i - ((selection / 8) * 8)].gameObject.SetActive(false);
                leftSideNames[i - ((selection / 8) * 8)].gameObject.SetActive(false);
            }
        }

        //Hide the selectors except for the thing that the player has selected
        foreach (Image item in leftSideBackgrounds)
        {
            item.enabled = false;
        }
        leftSideBackgrounds[selection%8].enabled = true;

        //Page number
        pageIndex.text = "Page " + ((selection / 8) + 1) + "/" + ((gm.allFish.Count / 8) + 1);
    }

    //Do you have a fish?
    public bool HasFish(int index)
    {
        return (playerStats.discoveredFish.Contains(gm.allFish[index].GetComponent<Fish>().description));
    }

    //When A pressed, set the stuff on the right page to work
    public void DisplayFish(FishData fish)
    {
        //Set the image to show the fish
        rightSideImage.sprite = fish.sprite;
        rightSideImage.SetNativeSize();
        rightSideImage.rectTransform.sizeDelta *= 3;

        //Set the texts to the fish
        rightSideStats[0].text = fish.name;

        rightSideStats[1].text = fish.description;

        rightSideStats[2].text = fish.speed switch
        {
            <= 0.1f => "Extremely Fast",
            <= 0.2f => "Very Fast",
            <= 0.4f => "Fast",
            <= 0.7f => "Medium",
            <= 1f => "Slow",
            _ => "Very Slow",
        };
        rightSideStats[3].text = fish.escape.ToString() + " m/s";

        rightSideStats[4].text = fish.turnChance switch
        {
            0 => "Never",
            <= 1 => "Very Unlikely",
            <= 3 => "Unlikely",
            <= 10 => "Sometimes",
            <= 25 => "Often",
            <= 40 => "Very Often",
            _ => "Continuously",
        };

        rightSideStats[5].text = fish.skipChance switch
        {
            0 => "Never",
            <= 1 => "Very Unlikely",
            <= 3 => "Unlikely",
            <= 10 => "Sometimes",
            <= 25 => "Often",
            <= 40 => "Very Often",
            _ => "Continuously",
        };
        rightSideStats[6].text = "Base Value: $" + fish.value;
    }

    //Menu navigation
    public void Left(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            selection = Mathf.Clamp(selection -= 4, 0, gm.allFish.Count - 1);
        }
    }

    public void Right(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            selection = Mathf.Clamp(selection += 4, 0, gm.allFish.Count - 1);
        }
    }

    public void Up(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            selection = Mathf.Clamp(selection -= 1, 0, gm.allFish.Count - 1);
        }
    }

    public void Down(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            selection = Mathf.Clamp(selection += 1, 0, gm.allFish.Count - 1);
        }
    }

    //Select a fish if you have it
    public void A(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (HasFish(selection))
                DisplayFish(new FishData(gm.allFish[selection].GetComponent<Fish>()));
        }
    }
}

//Class for fish data
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

    //Make from given stats
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

    //Make from an actual fish script
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