using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu]
public class PlayerStatsEpic : ScriptableObject
{
    //Money, points, and the turtle
    public int money;
    public int food;
    public int points;
    public int day;
    public float finalTime;
    public float foodEffectiveness;
    public float turtleHunger;

    //Upgrades for the player
    public float lineSpeedHorizontal;
    public float lineSpeedVertical;
    public float strengthMult;
    public int bucketMaxVolume;
    public int lightTier;

    //Costs for the upgrades for the player
    public int foodCost;
    public int lineSpeedUpgradeCost;
    public int strengthMultUpgradeCost;
    public int bucketSizeCost;
    public int lightbulbCost;

    //Fish tracking
    public List<int> bucket;
    public List<string> discoveredFish;
    public int bucketVolume = 0;

    public void Reset()
    {
        //Set every variable to their defaults. Change these to rebalance
        money = 0;
        food = 0;
        points = 0;
        day = 1;
        turtleHunger = 100;
        foodEffectiveness = 25;

        lineSpeedHorizontal = 1;
        lineSpeedVertical = 1;
        strengthMult = 4;
        bucketMaxVolume = 100;
        lightTier = 0;

        lineSpeedUpgradeCost = 50;
        strengthMultUpgradeCost = 25;
        bucketSizeCost = 40;
        lightbulbCost = 30;

        bucket = new List<int>();
        discoveredFish = new List<string>();
    }

    //Add money - I mean fish - into the bucket
    public void BucketAdd(int value, int volume)
    {
        if (bucketVolume < bucketMaxVolume)
        {
            bucket.Add(value);
            bucketVolume += volume;
        }
    }

    //Make the turtle hungrier every day
    public void NewDay()
    {
        day++;
        foodEffectiveness /= 2;
    }

    //Cash out the bucket
    public void SellFish()
    {
        foreach (int item in bucket)
        {
            money += item;
        }
        bucket.Clear();
    }
    
    //Feeding returns bool so sound can be played only if you actually fed the turtle
    public bool Feed()
    {
        if (food > 0)
        {
            food--;
            turtleHunger += foodEffectiveness;
            return true;
        }
        else
            return false;
    }
}
