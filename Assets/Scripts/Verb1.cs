using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Verb1
{
    public string id;
    public Display display;

    public Verb1(string id, string display)
    {
        this.id = id;
        this.display = new Display(display);
    }
}