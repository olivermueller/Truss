﻿using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using Vuforia;

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
		if (gameState.initializedPLayers)
		{
			if (isTrainer)
			{
				if (gameState.nodeID == "0")
					return;
				if (iterator == null || iterator.ID != gameState.nodeID)
				{
					
					iterator = TaskModel.Instance.tasks.First(p => p.ID == gameState.nodeID);
					
					iterator.StartTask();
					
				}
				return;
			}

			if (!FindObjectsOfType<PlayerUnit>().Any(p => p.IsTrainer)) return;
			if (isFirst)
			{
				var player = FindObjectsOfType<PlayerUnit>().First(p => p.isLocalPlayer);
				isTrainer = player.IsTrainer;

				if (isTrainer) return;
				// Check if there is a start task data in the scene and use it as the starting point. If there are none, use the first task that does not have any task pointing at it.
				iterator = (FindObjectOfType<StartTaskData>() != null) ? FindObjectOfType<StartTaskData>() : TaskModel.Instance.tasks.First(t => t._in == null);
				iterator = iterator.NextTask();
				player.CmdSetId(iterator.ID);
				isFirst = false;
			}
			else if(gameState.isDenied && !isTrainer)
			{
				print("Changed iterator" + " IsTrainer: " + isTrainer);
				print("Game State Node ID" + gameState.nodeID);
				var player = FindObjectsOfType<PlayerUnit>().First(p => p.isLocalPlayer);

				foreach (var t in FindObjectsOfType<TaskData>())
				{
					if (t.ID == gameState.nodeID)
					{
						iterator = t;
						break;
					}
				}

				
				print("Iterator ID" + iterator.ID);

				gameState.isDenied = false;
				player.CmdResetBools();
				iterator.StartTask();
				StartCoroutine(ActivateSubtask());

				return;
			}

			if (iterator != null)
			{
				//Debug.Log("iterator type: " + iterator.GetType().FullName);
				var val = iterator.IsCompleted();

				if (gameState.isApproved && gameState.isAwating)
				{
					gameState.isApproved = false;
					gameState.isAwating = false;
					var player = FindObjectsOfType<PlayerUnit>().First(p => p.isLocalPlayer);
					Debug.Log("Switching Task");
					var prevIterator = iterator;
					iterator = iterator.NextTask();
					if (iterator == null)
					{
						print("=============Moving back to the top");
						//var nestedNodes = FindObjectsOfType<NestedTaskData>();


						iterator = prevIterator.parentTask;
						(iterator as NestedTaskData).completedTasks ++;

						var subTaskEntry = (iterator as NestedTaskData).subTaskEntries.First(t => t.isSelected);
						subTaskEntry.isCompleted = true;
						subTaskEntry.isSelected = false;
						iterator.StartTask();
						
					}
					
					player.CmdSetId(iterator.ID);
					player.CmdResetBools();
					
					//player.TraineeNext();
				}
				else if (val.HasValue && val.Value && !gameState.isApproved && gameState.isAwating && gameState.isDenied)
				{
//					gameState.isApproved = false;
//					gameState.isAwating = false;
//					
//					Debug.Log("DENIED TASK");
//					var answerTaskData = (iterator as AnswerTaskData);
//					var answerTargetTaskData = (iterator as AnswerTargetTaskData);
//					if (answerTaskData != null)
//					{
//						var player = FindObjectsOfType<PlayerUnit>().First(p => p.isLocalPlayer);
//						iterator = answerTaskData.noTask;
//						iterator.StartTask();
//						player.CmdSetId(iterator.ID);
//						player.CmdResetBools();
//					}
//					else if (answerTargetTaskData != null) answerTargetTaskData.StartNoTask();
				}
				
				
			}			
			else
			{
				//Debug.Log("All Tasks completed!");

			}
		}
	}

	IEnumerator ActivateSubtask()
	{
		yield return  new WaitForEndOfFrame();
		var a = iterator as AnswerTargetTaskData;
		if (a)
		{
			print("--------Entered");
			var gameState = FindObjectOfType<NetworkedGameState>();
			print("--------changing stuff to true");
			//gameState.GetComponent<MissionTrackableEventHandler>().OnTrackableStateChange.Invoke(true);
			//gameState.GetComponent<MissionTrackableEventHandler>().OnTrackableStateChanged(TrackableBehaviour.Status.DETECTED, TrackableBehaviour.Status.TRACKED);
			a._finished = true;
			gameState.YesButton.gameObject.SetActive(true);
		}
	}


}
