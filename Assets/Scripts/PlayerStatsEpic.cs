using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu]
public class PlayerStatsEpic : ScriptableObject
{
    public int money;
    public int food;
    public int points;
    public int day;
    public float finalTime;
    public float foodEffectiveness;
    public float turtleHunger;

    public float lineSpeedHorizontal;
    public float lineSpeedVertical;
    public float strengthMult;
    public int bucketSize;
    public int lightTier;
    public int candleStatus;

    public int foodCost;
    public int lineSpeedUpgradeCost;
    public int strengthMultUpgradeCost;
    public int bucketSizeCost;
    public int lightbulbCost;

    public List<int> bucket;
    public List<string> discoveredFish;

    public void Reset()
    {
        money = 0;
        food = 0;
        points = 0;
        day = 1;
        turtleHunger = 100;
        foodEffectiveness = 25;

        lineSpeedHorizontal = 1;
        lineSpeedVertical = 1;
        strengthMult = 4;
        bucketSize = 3;
        lightTier = 0;
        candleStatus = 0;

        lineSpeedUpgradeCost = 40;
        strengthMultUpgradeCost = 30;
        bucketSizeCost = 60;
        lightbulbCost = 50;

        bucket = new List<int>();
        discoveredFish = new List<string>();
    }

    public void BucketAdd(int value)
    {
        if (bucket.Count < bucketSize)
            bucket.Add(value);
    }

    public void NewDay()
    {
        day++;
        foodEffectiveness /= 2;
    }

    public void SellFish()
    {
        foreach (int item in bucket)
        {
            money += item;
        }
        bucket.Clear();
    }

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
