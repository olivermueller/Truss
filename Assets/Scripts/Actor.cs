using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Actor
{
    public string mbox;
    public string name;

    public Actor(string name, string mbox)
    {
        this.name = name;
        this.mbox = mbox;
    }
}
