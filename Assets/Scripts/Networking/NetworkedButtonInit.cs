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
		Button button = GetComponent<Button>();
		button.onClick.RemoveAllListeners();
		
		var player = FindObjectsOfType<PlayerUnit>().First(p=>p.isLocalPlayer);

		if (player.isServer)
		{
			button.onClick.AddListener(() => player.CmdTrainerApproved());
		}
		else
		{
			button.onClick.AddListener(() => player.CmdTraineeNext());
		}
		
	}

}
