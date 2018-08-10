using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingScript : MonoBehaviour {
	TaskData iterator;

	bool isFirst = true;
	bool finished = false;

	void Awake () 
	{
		Debug.Log("Elements in task list: " + TaskModel.Instance.tasks.Count);
		
		
	}
	
	void Update () 
	{
		if(isFirst)
		{
			iterator = TaskModel.Instance.tasks[0];
			iterator.StartTask();
			isFirst = false;
		}

		if (iterator!=null)
		{
			//Debug.Log("iterator type: " + iterator.GetType().FullName);
			var val = iterator.IsCompleted();
			if (val.HasValue && val.Value)
			{
				iterator = iterator.NextTask();
			}
		}
		else
		{
			Debug.Log("All Tasks completed!");
		}
	} 

}
