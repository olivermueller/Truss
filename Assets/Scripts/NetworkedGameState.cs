using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using UnityEngine.UI;
using Utils;
[System.Serializable]
public class NetworkedGameState : NetworkBehaviour
{
	public Button YesButton, NoButton;
	private PlayerUnit playerUnit;
	
	[SyncVar] public bool isAwating = false;
	[SyncVar] public bool isApproved = false;
	[SyncVar] public string nodeID;
	[SyncVar] public string testString;
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
//		IdEvent.AddListener(player.SetNode);
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
	public void CmdSetNodeId(string val)
	{
		nodeID = val;
	}
	
	[Command]
	public void CmdSetApproved(bool val)
	{
		isApproved = val;
		//RpcUITrainerApproved();
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
		
		if(player.IsTrainer)
			YesButton.gameObject.SetActive(isAwating);
		else
			YesButton.gameObject.SetActive(!isAwating);
	}

	[ClientRpc]
	public void RpcUITrainerApproved()
	{
		var player = FindObjectsOfType<PlayerUnit>().First(p=>p.isLocalPlayer);
		
		if(player.IsTrainer)
			YesButton.gameObject.SetActive(!isApproved);
		else
			YesButton.gameObject.SetActive(isApproved);
	}

	private PlayerUnit player;
	
	private void Update()
	{
		if(!player) player = FindObjectsOfType<PlayerUnit>().First(p=>p.isLocalPlayer);

		if (player.IsTrainer)
		{
			if (isAwating && !isApproved)
			{
				YesButton.gameObject.SetActive(true);
			}
			else
			{
				YesButton.gameObject.SetActive(false);
			}

		}
		else
		{
			if (!isApproved && !isAwating)
			{
				YesButton.gameObject.SetActive(true);
			}
			else
			{
				YesButton.gameObject.SetActive(false);
			}

		}
		
	}
	
	
}
