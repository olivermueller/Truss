using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using Vuforia;

public class LoadSceneScript : NetworkBehaviour {
	private void Awake()
	{
		VuforiaRuntime.Instance.InitVuforia();
	}

	public void LoadScene(int sceneToLoad)
	{
		
		StartCoroutine(StopHostAfter(2));
	}

	IEnumerator StopHostAfter(int t)
	{
		VuforiaRuntime.Instance.Deinit();
		yield return new WaitForSeconds(t);
		NetworkManager.singleton.StopHost();
		SceneManager.LoadScene(0);
	}

	private void OnDestroy()
	{
		if (isLocalPlayer && !isServer)
		{
			VuforiaRuntime.Instance.Deinit();
		}
	}
}
