using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CheckItem
{
    public string name;
    public bool isCompleted;
    public GameObject animationObject;
    public void LoadTask()
    {
        Canvas canvas = GameObject.FindObjectOfType<Canvas>();

        GameObject obj = GameObject.Instantiate(animationObject);
        obj.transform.position = TargetManager.Instance.currentlyActive.transform.position;
    }
}
