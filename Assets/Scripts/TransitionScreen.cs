using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TransitionScreen : MonoBehaviour
{
    public PlayerStatsEpic playerStats;

    public TextMeshProUGUI dayCounter;
    public TextMeshProUGUI moneyCounter;
    public TextMeshProUGUI foodCounter;

    private void Update()
    {
        dayCounter.text = "Day " + playerStats.day;
        moneyCounter.text = "$" + playerStats.money;
        foodCounter.text = playerStats.turtleFood + "/" + playerStats.turtleHunger;
    }
}
