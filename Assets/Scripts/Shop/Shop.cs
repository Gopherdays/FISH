using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Shop : MonoBehaviour
{
    public TextMeshProUGUI textbox;
    public string[] dialogue;
    public int index;
    string currentDialogue;
    string remainingDialogue;
    enum Status
    {
        Normal,
        TryLeave,
        Leave
    }
    Status status;

    private void FixedUpdate()
    {
        if (remainingDialogue.Length > 0)
        {
            currentDialogue += remainingDialogue[0];
            remainingDialogue = remainingDialogue[1..];
        }
        if (status == Status.TryLeave && index != 1)
        {
            Dialogue(1);
        }
        if (status == Status.Normal)
        {

        }
    }

    void Dialogue(int index)
    {
        currentDialogue = "";
        remainingDialogue = dialogue[index];
    }
}
