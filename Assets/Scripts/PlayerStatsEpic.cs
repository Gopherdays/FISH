using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PlayerStatsEpic : ScriptableObject
{
    public int money;
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

    public void Reset()
    {
        money = 0;
        turtleFood = 0;
        turtleHunger = 30;
        lineSpeedHorizontal = 1;
        lineSpeedVertical = 1;
        lineSpeedUpgradeCost = 100;
    }

    public void NewDay()
    {
        if (turtleFood < turtleHunger)
        {
            // KILL YOU
            Application.Quit();
        }
        else
        {
            turtleHunger *= 2;
            turtleFood = 0;
        }
    }
}
