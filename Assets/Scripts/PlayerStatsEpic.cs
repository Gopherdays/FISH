using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu]
public class PlayerStatsEpic : ScriptableObject
{
    public int money;
    public int points;
    public int day;
    public int foodEffectiveness;
    public float turtleHunger;

    public float lineSpeedHorizontal;
    public float lineSpeedVertical;
    public float strengthMult;
    public int bucketSize;
    public int lightTier;
    public int candleStatus;

    public int lineSpeedUpgradeCost;
    public int strengthMultUpgradeCost;
    public int bucketSizeCost;
    public int lightbulbCost;

    public List<int> bucket;

    public void Reset()
    {
        money = 0;
        points = 0;
        day = 0;
        turtleHunger = 100;
        foodEffectiveness = 25;

        lineSpeedHorizontal = 1;
        lineSpeedVertical = 1;
        strengthMult = 1;
        bucketSize = 5;
        lightTier = 0;
        candleStatus = 0;

        lineSpeedUpgradeCost = 100;
        strengthMultUpgradeCost = 60;
        bucketSizeCost = 150;
        lightbulbCost = 120;

        bucket = new();
}

    public void NewDay()
    {
        day++;
        foodEffectiveness /= 2;
    }

    public void Feed()
    {
        turtleHunger += foodEffectiveness;
    }
}
