using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class NetworkedButtonInit : NetworkBehaviour
{
	
	void Start ()
	{

		StartCoroutine("InitAfter");
	}

	IEnumerator InitAfter()
	{
		yield return new WaitForFixedUpdate();
		Button button = GetComponent<Button>();
		button.onClick.RemoveAllListeners();
		
		var player = FindObjectsOfType<PlayerUnit>().First(p=>p.isLocalPlayer);

		if (player.IsTrainer)
		{
			button.onClick.AddListener(() => player.CmdTrainerApproved());
		}
		else
		{
			button.onClick.AddListener(() => player.CmdTraineeNext());
		}
	}

}
