﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Utils;

public class NetworkedGameState : NetworkBehaviour
{
	public Button YesButton, NoButton;
	private PlayerUnit playerUnit;
	
	[SyncVar] public bool isAwating = false;
	[SyncVar] public bool isApproved = false;
	[SyncVar] public int nodeID;

	private void Start()
	{
		YesButton = GameObject.FindWithTag("CanvasYes").GetComponent<Button>();
		NoButton = GameObject.FindWithTag("CanvasNo").GetComponent<Button>();
		StartCoroutine(InitAfter());
		
	}

	IEnumerator InitAfter()
	{
		yield return new WaitUntil(()=>FindObjectsOfType<PlayerUnit>().Any(p=>p.IsTrainer));
		
		var player = FindObjectsOfType<PlayerUnit>().First(p=>p.isLocalPlayer);
		if (player.IsTrainer)
		{
			YesButton.gameObject.SetActive(false);
			NoButton.gameObject.SetActive(false);
			
			YesButton.onClick.AddListener(() => player.CmdTrainerApproved());
		}
		else
		{
			YesButton.gameObject.SetActive(true);
			NoButton.gameObject.SetActive(false);
		
			YesButton.onClick.AddListener(() => player.CmdTraineeNext());
		}
		
		YesButton.interactable = true;
		NoButton.interactable = true;
		
	}
	
	[Command]
	public void CmdSetAwating(bool val)
	{
		isAwating = val;
		//RpcUITraineeNext();
	}
	
	[Command]
	public void CmdSetApproved(bool val)
	{
		isApproved = val;
		//RpcUITrainerApproved();
	}
	
	[Command]
	public void CmdSetNodeID(int val)
	{
		nodeID = val;
	}

	[ClientRpc]
	void RpcUITraineeNext()
	{
		var player = FindObjectsOfType<PlayerUnit>().First(p=>p.isLocalPlayer);
		
		if(player.IsTrainer)
			YesButton.gameObject.SetActive(isAwating);
		else
			YesButton.gameObject.SetActive(!isAwating);
	}

	[ClientRpc]
	void RpcUITrainerApproved()
	{
		var player = FindObjectsOfType<PlayerUnit>().First(p=>p.isLocalPlayer);
		
		if(player.IsTrainer)
			YesButton.gameObject.SetActive(!isApproved);
		else
			YesButton.gameObject.SetActive(isApproved);
	}
}
