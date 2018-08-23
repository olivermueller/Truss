using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
			// Check if there is a start task data in the scene and use it as the starting point. If there are none, use the first task that does not have any task pointing at it.
			iterator = (FindObjectOfType<StartTaskData>()!=null) ? FindObjectOfType<StartTaskData>() : TaskModel.Instance.tasks.First(t => t._in == null);
			iterator.StartTask();
			isFirst = false;
		}

		if (iterator!=null)
		{
			//Debug.Log("iterator type: " + iterator.GetType().FullName);
			var val = iterator.IsCompleted();
			if (val.HasValue)
			{
				if(val.Value) iterator = iterator.NextTask();
				else
				{
					Debug.Log("no task found");
					iterator = (iterator as AnswerTaskData).StartNoTask();

				}
			}
			
		}
		else
		{
			//Debug.Log("All Tasks completed!");
			
		}
	} 

}
