using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Obj
{
    public string id;
    public Definition definition;

    public Obj(string id, string definitionName, string definitionDescription)
    {
        this.id = id;
        this.definition = new Definition(definitionName, definitionDescription);
    }

}
