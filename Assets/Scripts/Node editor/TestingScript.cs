using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class TestingScript : NetworkBehaviour {
	public TaskData iterator;

	bool isFirst = true;
	bool finished = false;
	private NetworkedGameState gameState;

	private bool isTrainer;
	void Awake () 
	{
		Debug.Log("Elements in task list: " + TaskModel.Instance.tasks.Count);
		gameState = GetComponent<NetworkedGameState>();
		
	}
	
	void Update () 
	{
		if (isTrainer) return;
		if(!FindObjectsOfType<PlayerUnit>().Any(p=>p.IsTrainer)) return;
		if(isFirst)
		{
			var player = FindObjectsOfType<PlayerUnit>().First(p=>p.isLocalPlayer);
			isTrainer = player.IsTrainer;
			
			if(isTrainer) return;
			// Check if there is a start task data in the scene and use it as the starting point. If there are none, use the first task that does not have any task pointing at it.
			iterator = (FindObjectOfType<StartTaskData>()!=null) ? FindObjectOfType<StartTaskData>() : TaskModel.Instance.tasks.First(t => t._in == null);
			
			iterator = iterator.NextTask();
			isFirst = false;
		}

		if (iterator!=null)
		{
			//Debug.Log("iterator type: " + iterator.GetType().FullName);
			var val = iterator.IsCompleted();
			if (val.HasValue)
			{
				if (val.Value && gameState.isApproved && gameState.isAwating)
				{
					Debug.Log("Switching Task");
					//iterator = iterator.NextTask();


					CmdResetBools();
					
					var player = FindObjectsOfType<PlayerUnit>().First(p=>p.isLocalPlayer);
					//player.TraineeNext();
				}
				else if(!val.Value && gameState.isApproved && gameState.isAwating)
				{
					Debug.Log("no task found");
					var answerTaskData = (iterator as AnswerTaskData);
					var answerTargetTaskData = (iterator as AnswerTargetTaskData);
					if (answerTaskData != null) answerTaskData.StartNoTask();
					else if (answerTargetTaskData != null) answerTargetTaskData.StartNoTask();
				}
			}
			
		}
		else
		{
			//Debug.Log("All Tasks completed!");
			
		}
	}

	[Command]
	void CmdResetBools()
	{
		gameState.CmdSetApproved(false);
		gameState.CmdSetAwating(false);
	}
}
