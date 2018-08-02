using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskTracker : MonoBehaviour {
    
    Task iterator;
    public GameObject[] animationObjects, targetObjects;


    bool isFirst = true;
    bool finished = false;
	void Start () 
    {

        List<Task> tasks = new List<Task>();
        for (int i = 0; i < targetObjects.Length; i++)
        {
            tasks.Add(new TargetTask("Ïmagetarget " + i, "test", animationObjects[i], targetObjects[i]));
            tasks.Add(new YesNoTask("YesNo  " + i, "test", animationObjects[i], targetObjects[i]));
        }

        for (int i = 0; i < tasks.Count - 1; i++)
        {
            tasks[i].SetNextTask(tasks[i+1]);
        }        
        iterator = tasks[0];

	}

	void Update () 
    {
        if(isFirst)
        {
            iterator.Start();
            isFirst = false;
        }
        var val = iterator.IsCompleted();
        if (!finished && val.HasValue && val.Value)
        {
            iterator = iterator.NextTask();
            if (iterator == null) finished = true;
            Debug.Log("ŸAAAAAAS");
        }
	}
}
