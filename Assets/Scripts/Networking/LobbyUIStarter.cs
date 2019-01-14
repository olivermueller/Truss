using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyUIStarter : MonoBehaviour
{

	public GameObject MainPanel;

	private void Awake()
	{
		MainPanel.SetActive(true);
	}
}
