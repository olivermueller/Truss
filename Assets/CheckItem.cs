using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SubTask
{
    public string question;
}

[System.Serializable]
public class CheckItem
{
    public string name;
    public bool isCompleted;
    public GameObject animationObject;
    public List<GameObject> requiredComponents;
    public string question;
    public List<SubTask> subTask;
    public int subTaskIndex = 0;
    public void LoadTask()
    {
        Canvas canvas = GameObject.FindObjectOfType<Canvas>();

        GameObject obj = GameObject.Instantiate(animationObject);
        obj.transform.position = TargetManager.Instance.currentlyActive.transform.position;
    }
}
