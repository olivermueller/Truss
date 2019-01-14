using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class LoadSceneScript : MonoBehaviour {

	public void LoadScene(int sceneToLoad)
	{
		NetworkManager.Shutdown();

	}
}
