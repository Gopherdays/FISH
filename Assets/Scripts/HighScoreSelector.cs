using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class HighScoreSelector : MonoBehaviour
{
    public GameManager gm;
    public GameObject window;
    public RectTransform selectionHighlight;
    public TextMeshProUGUI[] textBoxes = new TextMeshProUGUI[4];

    public string[] allLetters;

    public int[] letters = new int[4];
    private int selector;

    private void Start()
    {
        letters[0] = 0;
        letters[1] = 0;
        letters[2] = 0;
        letters[3] = 0;

        selector = 0;
    }

    private void Update()
    {
        for (int i = 0; i < 4; i++)
        {
            textBoxes[i].text = allLetters[letters[i] % allLetters.Length];
        }
        selectionHighlight.localPosition = new(-300 + (200 * selector), selectionHighlight.localPosition.y);
        print(selector);
    }

    public void A(InputAction.CallbackContext context)
    {
        if (window.activeSelf)
        {
            gm.SaveCurrentStats(allLetters[letters[0] % allLetters.Length] + allLetters[letters[1] % allLetters.Length] + allLetters[letters[2] % allLetters.Length] + allLetters[letters[3] % allLetters.Length]);
            window.SetActive(false);
        }
        else
            gm.GoMenu();
    }
    public void B(InputAction.CallbackContext context)
    {
        if (window.activeSelf)
            window.SetActive(false);
        else
            gm.GoEncyclopedia();
    }
    public void Up(InputAction.CallbackContext context)
    {
        if (context.started)
            letters[selector] -= 1;
        if (letters[selector] < 0)
            letters[selector] = allLetters.Length - 1;
    }
    public void Down(InputAction.CallbackContext context)
    {
        if (context.started)
            letters[selector] += 1;
    }
    public void Left(InputAction.CallbackContext context)
    {
        if (context.started)
            selector = Mathf.Clamp(selector-1, 0, 3);
        print(Mathf.Clamp(selector, 0, 3));
    }
    public void Right(InputAction.CallbackContext context)
    {
        if (context.started)
            selector = Mathf.Clamp(selector+1, 0, 3);
        print(Mathf.Clamp(selector, 0, 3));
    }
}
