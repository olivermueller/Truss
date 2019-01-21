using System.Collections;
using System.Collections.Generic;
using Prototype.NetworkLobby;
using UnityEngine;
using UnityEngine.UI;

public class LobbyInputValidation : MonoBehaviour
{

	public Text inputText;
	public LobbyMainMenu lobbyMainMenu;
	
	void Start()
	{
		GetComponent<Button>().onClick.AddListener(ValidateInput);
	}
	
	public void ValidateInput()
	{
		if (inputText.text.Length > 0)
		{
			inputText.text = inputText.text.ToUpper();
			lobbyMainMenu.OnClickCreateMatchmakingGame();
		}
	}
}
