﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using UnityEngine.UI;
//using Utils;
[System.Serializable]
public class NetworkedGameState : NetworkBehaviour
{
	public Button YesButton, NoButton;
	private PlayerUnit playerUnit;
	public TestingScript _testingScript;


	public int value;
	
	[SyncVar] public bool isAwating = false;
	[SyncVar] public bool isApproved = false;
	[SyncVar] public bool isDenied = false;
	[SyncVar] public string nodeID;
	[SyncVar] public string testString;
	private void Start()
	{
		
		StartCoroutine(InitAfter());
		
	}
	
	public bool initializedPLayers = false;
	IEnumerator InitAfter()
	{
		yield return new WaitUntil(()=>FindObjectsOfType<PlayerUnit>().Any(p=>p.IsTrainer));
		YesButton = GameObject.FindWithTag("CanvasYes").GetComponent<Button>();
		NoButton = GameObject.FindWithTag("CanvasNo").GetComponent<Button>();
		var player = FindObjectsOfType<PlayerUnit>().First(p=>p.isLocalPlayer);
		
		if (player.IsTrainer)
		{
			YesButton.gameObject.SetActive(false);
			NoButton.gameObject.SetActive(false);
			
			YesButton.onClick.AddListener(() => player.CmdTrainerApproved());
			NoButton.onClick.AddListener(() => player.CmdTrainerNotApproved());
		}
		else
		{
			YesButton.gameObject.SetActive(true);
		
			YesButton.onClick.AddListener(() => player.CmdTraineeNext());
		}
//		IdEvent.AddListener(player.SetNode);
		YesButton.interactable = true;
		NoButton.interactable = true;
		initializedPLayers = true;

	}
	
	[Command]
	public void CmdSetAwating(bool val)
	{
		isAwating = val;
		//RpcUITraineeNext();
	}
	
	[Command]
	public void CmdSetNodeId(string val)
	{
		nodeID = val;
	}
	
	[Command]
	public void CmdSetApproved(bool val)
	{
		isApproved = val;
	}
	[Command]
	public void CmdSetDenied(bool val)
	{
		isDenied = val;
	}

	[ClientRpc]
	public void RpcChangeTrainerIterator()
	{
		var player = FindObjectsOfType<PlayerUnit>().First(p=>p.isLocalPlayer);
		if (player.IsTrainer)
		{
			GetComponent<TestingScript>().iterator = TaskModel.Instance.tasks.First(p => p.ID == nodeID);
			GetComponent<TestingScript>().iterator.StartTask();
		}
	}

	[Command]
	public void CmdUITraineeNext()
	{
		RpcUITraineeNext();
	}
	
	[ClientRpc]
	public void RpcUITraineeNext()
	{
		var player = FindObjectsOfType<PlayerUnit>().First(p=>p.isLocalPlayer);

		if (player.IsTrainer)
		{
			YesButton.gameObject.SetActive(isAwating);
			NoButton.gameObject.SetActive(isAwating);
		}
		else
		{
			YesButton.gameObject.SetActive(!isAwating);
			
		}
	}

	[ClientRpc]
	public void RpcUITrainerApproved()
	{
		var player = FindObjectsOfType<PlayerUnit>().First(p=>p.isLocalPlayer);

		if (player.IsTrainer)
		{
			YesButton.gameObject.SetActive(!isApproved);
			NoButton.gameObject.SetActive(!isApproved);
		}
		else
			YesButton.gameObject.SetActive(isApproved);
	}

	private PlayerUnit player;
	
	private void Update()
	{
		if (initializedPLayers)
		{
			if (!player)
			{
				player = FindObjectsOfType<PlayerUnit>().First(p => p.isLocalPlayer);
			}


			if (player.IsTrainer)
			{
				if (_testingScript == null)
				{
					_testingScript = GetComponent<TestingScript>();
				}
				if (isAwating && !isApproved)
				{
					YesButton.gameObject.SetActive(true);
					NoButton.gameObject.SetActive(true);
				}
				else
				{
					YesButton.gameObject.SetActive(false);
					NoButton.gameObject.SetActive(false);

				}

			}
			else
			{
				if (_testingScript == null)
				{
					_testingScript = GetComponent<TestingScript>();
				}
				if (!isApproved && !isAwating && _testingScript.iterator != null &&
				    _testingScript.iterator.IsCompleted().HasValue)
				{
					var completed = _testingScript.iterator.IsCompleted();
					if(completed.HasValue) YesButton.gameObject.SetActive(completed.Value);
					 
				}
				else
				{
					YesButton.gameObject.SetActive(false);
					NoButton.gameObject.SetActive(false);
				}

			}

		}

	}
	
	
	
	
}
