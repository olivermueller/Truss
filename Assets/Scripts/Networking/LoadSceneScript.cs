using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class LoadSceneScript : NetworkBehaviour {

	public void LoadScene(int sceneToLoad)
	{
		NetworkManager.singleton.StopHost();

	}
}
