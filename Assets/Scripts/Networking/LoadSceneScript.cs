using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using Vuforia;

public class LoadSceneScript : NetworkBehaviour {

	public void LoadScene(int sceneToLoad)
	{
		StartCoroutine(StopHostAfter(2));
	}

	IEnumerator StopHostAfter(int t)
	{
		NetworkManager.singleton.StopHost();
		yield return new WaitForSeconds(t);
		
		SceneManager.LoadScene(0);
	}
}
