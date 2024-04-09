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
    public int turtleFood;
    public int turtleHunger;

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
        day = 0;
        turtleFood = 0;
        turtleHunger = 0;

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
        if (turtleFood < turtleHunger)
        {
            // kill you less
            SceneManager.LoadScene(0);
        }
        else
        {
            turtleHunger *= 2;
            turtleFood = 0;
            day++;
        }
    }
}
