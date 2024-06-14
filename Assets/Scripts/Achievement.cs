using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Achievement : ScriptableObject
{
    public new string name;
    public string desc;
    public string flavor;
    public Sprite icon;
    public int progressMax = 1;
    public bool hidden = false;
}
