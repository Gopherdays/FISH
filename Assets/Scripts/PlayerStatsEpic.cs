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

        lineSpeedUpgradeCost = 100;
        strengthMultUpgradeCost = 60;
        bucketSizeCost = 150;
        lightbulbCost = 120;

        bucket = new List<int>();
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

    public void Feed()
    {
        if (food > 0)
        {
            Debug.Log("Yes food.");
            food--;
            turtleHunger += foodEffectiveness;
        }
        else
            Debug.Log("No food.");
    }
}
