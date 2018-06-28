using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Definition
{
    public Display name;
    public Display description;

    public Definition(string name, string description)
    {
        this.name = new Display(name);
        this.description = new Display(description);
    }
}
