using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TaskTrackerDummy : MonoBehaviour {
    
	TaskData iterator;
	public GameObject[] animationObjects, targetObjects;


	bool isFirst = true;
	bool finished = false;
	void Start () 
	{
//		Task p1 = new TargetTask("Task 1", "Locate "+targetObjects[2].name, animationObjects[0], targetObjects[2]);
//		Task p2 = new TargetTask("Task 2", "Locate "+targetObjects[0].name, animationObjects[0], targetObjects[0]);
//		p1.SetNextTask(p2);
//		var root = new SubTaskTask("Task 3", "Complete the subtasks", animationObjects[0], targetObjects[0]);
//		p2.SetNextTask(root);
//		Task n1 = new TargetTask("Task 4", "Locate target 1", animationObjects[0], targetObjects[0]);
//		root.SetNextTask(n1);
//		Task sn1 = new TargetTask("Target sub task 1,1,1", "Locate "+targetObjects[0].name, animationObjects[0], targetObjects[0]);
//		Task sn2 = new TargetTask("Target sub task 1,1,2", "Locate "+targetObjects[1].name, animationObjects[0], targetObjects[1]);
//		Task sn3 = new TargetTask("Target sub task 1,2,1", "Locate "+targetObjects[2].name, animationObjects[0], targetObjects[2]);
//		Task sn4 = new TargetTask("Target sub task 1,2,2", "Locate "+targetObjects[3].name, animationObjects[0], targetObjects[3]);
//		sn1.SetNextTask(sn2);
//		sn3.SetNextTask(sn4);
//		root.AddSubTask(sn1);
//		root.AddSubTask(sn3);
//		iterator = p1;
	}

	void Update () 
	{
//		if(isFirst)
//		{
//			iterator.Start();
//			isFirst = false;
//		}
//
//		if (iterator!=null)
//		{
//			var val = iterator.IsCompleted();
//			if (!finished && val.HasValue && val.Value)
//			{
//				Debug.Log("<color=green>Completed task: " + iterator._title+"</color>" );
//				iterator = iterator.NextTask();
//				if (iterator == null) finished = true;
//				Debug.Log("ŸAAAAAAS");
//			}
//		}
//		else
//		{
//			Debug.Log("All Tasks completed!");
//		}
	}
}
