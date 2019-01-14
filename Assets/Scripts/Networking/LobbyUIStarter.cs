using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LobbyUIStarter : MonoBehaviour
{

	public GameObject MainPanel;

	private void Awake()
	{
		NetworkManager.Shutdown();
	}
}
