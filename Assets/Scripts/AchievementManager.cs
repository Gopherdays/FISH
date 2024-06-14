using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AchievementManager : MonoBehaviour
{
    public List<Achievement> themChievos;
    public List<int> progresses;
    public GameObject achievementPopup;

    private void Start()
    {
        if (progresses == null)
            progresses = new(themChievos.Count);
    }

    public void Progress(int index, int by = 1)
    {
        if (themChievos.Count >= index)
            index = 0;
        progresses[index] += by;
        if (progresses[index] >= themChievos[index].progressMax)
        {
            GameObject temp = Instantiate(achievementPopup,GameObject.Find("Transition Scene").transform);
            temp.GetComponent<AchievementPopup>().achievement = themChievos[index];
        }
    }
}