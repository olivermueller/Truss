using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Utils;

public class NetworkedGameState : NetworkBehaviour
{
	public Button YesButton, NoButton;

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
		}
		else
		{
			YesButton.gameObject.SetActive(true);
			NoButton.gameObject.SetActive(false);
		}
		
		YesButton.interactable = true;
		NoButton.interactable = true;
	}

	[Command]
	public void CmdSetAwating(bool val)
	{
		isAwating = val;
	}
	
	[Command]
	public void CmdSetApproved(bool val)
	{
		isApproved = val;
	}
	
	[Command]
	public void CmdSetNodeID(int val)
	{
		nodeID = val;
	}
}
