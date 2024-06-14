using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManagement : MonoBehaviour
{
    public GameManager gm;
    public GameObject selector;
    public GameObject[] windows;
    public int index;
    public bool isWindow;

    public void MenuButtonClicked(ButtonFunctions func)
    {
        switch (func)
        {
            case ButtonFunctions.StartGame:
                gm.GoFish();
                break;
            case ButtonFunctions.OpenOptions:
                windows[0].SetActive(!windows[0].activeSelf);
                break;
            case ButtonFunctions.OpenCredits:
                windows[1].SetActive(!windows[1].activeSelf);
                break;
            case ButtonFunctions.OpenAchievements:
                gm.GoAchievement();
                break;
            case ButtonFunctions.OpenEncyclopedia:
                gm.GoEncyclopedia();
                break;
            case ButtonFunctions.EndCurrentGame:
                gm.GoMenu();
                break;
            case ButtonFunctions.CloseGame:
                Application.Quit();
                break;
            default:
                break;
        }
    }
}

public enum ButtonFunctions
{
    StartGame,
    OpenOptions,
    OpenCredits,
    OpenAchievements,
    OpenEncyclopedia,
    EndCurrentGame,
    CloseGame
}